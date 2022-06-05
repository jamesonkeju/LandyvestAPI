using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Landyvest.Data;
using Landyvest.Services.AuditLog.Concrete;
using Landyvest.Services.CommonRoute;
using Landyvest.Services.CommonRoute.concrete;
using Landyvest.Services.Country;
using Landyvest.Services.Country.Concrete;
using Landyvest.Services.Permission;
using Landyvest.Services.Permission.Concrete;
using Landyvest.Services.Permission.Interface;
using Landyvest.Services.Role.Concrete;
using Landyvest.Services.Role.Interface;
using Landyvest.Services.SystemSetting.Concrete;
using Landyvest.Services.SystemSetting.Interface;
using Landyvest.Services.User.Interface;
using Landyvest.Services.User.Concrete;
using Landyvest.Services.FAQs.Interface;
using Landyvest.Services.FAQs.Concrete;
using Landyvest.Services.FileHandler;
using Landyvest.Services.Report.Interface;
using Landyvest.Services.Report.Concete;

namespace Landyvest.Services
{
    public class AppBootstrapper
    {
        private static void AutoInjectLayers(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan.FromCallingAssembly().AddClasses(classes => classes
                    .Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("Service")), false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

        }
        public static void InitServices(IServiceCollection services)
        {
            AutoInjectLayers(services);


            
            services.AddScoped<DbContext, LandyvestAppContext>();
            services.AddTransient(typeof(DbContextOptions<LandyvestAppContext>));
            //services.AddTransient<ShortCodeController>();
            

            services.AddTransient<IRole, RoleService>();
            services.AddTransient<IActivityLog, ActivityLogServices>();
            services.AddTransient<IPermission, PermissionServices>();
            services.AddTransient<IRolePermission, RolePermissionService>();
            services.AddTransient<ICountry, CountryServices>();

            services.AddTransient<IFAQs, FAQsServices>();
            services.AddTransient<IActivityLog, ActivityLogServices>();
      
            services.AddTransient<IPermission, PermissionServices>();
            services.AddTransient<IRolePermission, RolePermissionService>();
           

            // Domain.Core - Identity
            services.AddTransient<ICommonRoute, CommonRouteServices>();
          
            services.AddTransient<IPermission, PermissionServices>();

            services.AddTransient<ISystemSetting, SystemSettingService>();
          
            services.AddTransient<IUserManagement, UserManagementServices>();
            services.AddTransient<IReportManagement, ReportManagementService>();

           
            services.AddTransient<IFileHandler, FileHandlerServices>();
           

        }



    }
}
