using Abp.Extensions;
using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.SqlServer;
using Landyvest.Data;
using Landyvest.Data.Models;
using Landyvest.Services;
using Landyvest.Services.Emailing.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Landyvest.API
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private const string _apiVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEmailing emailManager, IConfiguration configuration, LandyvestAppContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            /////hang fire dashboard
            //  app.UseHangfireDashboard();
            //var email = new BackgroundJobs(emailManager, configuration, electricityShago,  context, processPayment);

            // RecurringJob.AddOrUpdate(() => email.ProcessEmail(), "*/15 * * * * *");
            // RecurringJob.AddOrUpdate(() => email.ProcessEkoRequery(), "*/20 * * * * *");

         

            app.UseRouting();
            //app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
               // Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            string baseApiUrl = Configuration.GetSection("BaseApiUrl").Value;

            // Enable middleware to serve generated Swagger as a JSON endpoint
            //app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
            app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint($"{_apiVersion}/swagger.json", "HR");
            //});

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Landyvest Application");
            });

            // setup app's root folders
            AppDomain.CurrentDomain.SetData("ContentRootPath", env.ContentRootPath);
            AppDomain.CurrentDomain.SetData("WebRootPath", env.WebRootPath);




            // Call Background Service with Hang-fire:
           //ConfigureHangFireJobs();



        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            var connect = config.GetSection("ConnectionStrings").Get<List<string>>().FirstOrDefault();

            services.AddDbContext<LandyvestAppContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
              .AddEntityFrameworkStores<LandyvestAppContext>()
              .AddUserManager<UserManager<ApplicationUser>>()
              .AddRoleManager<RoleManager<ApplicationRole>>()
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddUserStore<UserStore<ApplicationUser, ApplicationRole, LandyvestAppContext, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>()
              .AddRoleStore<RoleStore<ApplicationRole, LandyvestAppContext, string, ApplicationUserRole, IdentityRoleClaim<string>>>()
              .AddDefaultTokenProviders();
           


            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()


                .UseSqlServerStorage(connect, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Registering Application Services:
            AppBootstrapper.InitServices(services);

            #region HttpClient Factory for Http Serive Calls:

            services.AddHttpClient("TenantPayment", c =>
            {
                c.BaseAddress = new Uri(Configuration["PayStack:BaseUrl"]);
                // Paystack Payment API ContentType
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration["PayStack:SecretKey"]);
            });

            #endregion

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = context.Request.Headers["Accept"].ToString();
                            return Task.CompletedTask;
                            //string _message = "Authetication token is invalid.";
                        }
                    };

                   
                });

            services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);

            //services.AddTransient<ISessionTokenGenerator, JwtTokenGenerator>();
            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    name: _defaultCorsPolicyName,
                    builder => builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )

                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );


            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_apiVersion, new OpenApiInfo
                {
                    Version = _apiVersion,
                    Title = "Landyvest Manager API",
                    Description = "Landyvest Manager API",
           
                    Contact = new OpenApiContact
                    {
                        Name = "Landyvest Manager API",
                        Email = string.Empty,
                        Url = new Uri("http://www.vatebra.com/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("http://www.vatebra.com"),
                    }
                });
                options.DocInclusionPredicate((docName, description) => true);
                //bearerAuth
                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,

                });
                options.CustomSchemaIds(type => type.ToString());
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);
            });

            //services.AddControllers().AddNewtonsoftJson(options =>
            //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

        }

    }
}
