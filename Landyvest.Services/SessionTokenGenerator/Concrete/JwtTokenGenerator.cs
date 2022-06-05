using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Models;
using Landyvest.Data.Payload;
using Landyvest.Services.SessionTokenGenerator.Interface;

namespace Landyvest.Services.SessionTokenGenerator.Concrete
{
    public class JwtTokenGenerator : ISessionTokenGenerator
    {
        private IConfiguration _config;
        private readonly ILogger<JwtTokenGenerator> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenGenerator(IConfiguration configuration, ILogger<JwtTokenGenerator> logger, UserManager<ApplicationUser> userManager)
        {
            _config = configuration;
            _logger = logger;
            _userManager = userManager;
        }

        public string GenerateToken(VwUserInfornation session)
        {
            //var secretKey = Encoding.UTF8.GetBytes(_config["JWT:Secret"]);
            //var tokenLifeTime = _config["JWT:TokenLifeTime"];

            //var totalMinutes = Convert.ToInt32(tokenLifeTime);
            //var expiryTime = TimeSpan.FromMinutes(totalMinutes);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new List<Claim>
            //    {
            //        new Claim(nameof(VwUserInfornation.email), session.email),
            //        new Claim(nameof(VwUserInfornation.user_id), session.user_id.ToString() ?? ""),
            //        new Claim(nameof(VwUserInfornation.employee_id), session.employee_id.ToString() ?? "")
            //    }),
            //    Issuer = "",
            //    Expires = DateTime.UtcNow.Add(expiryTime),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
            return  null;
        }

        public async Task<string> GenerateToken(ApplicationUser usr)
        {
            //var signingCredentials = GetSigningCredentials();
            //var claims = await GetClaims(usr);
            //var token = GenerateTokenOptions(signingCredentials, claims);

            //return new JwtSecurityTokenHandler().WriteToken(token);
            return null;
        }



        #region Token Helpers:

        private DateTime GetExpiration(string lifeTime)
        {
            //var totalMinutes = Convert.ToInt32(lifeTime);
            //var expiryTime = TimeSpan.FromMinutes(totalMinutes);
            //return DateTime.UtcNow.Add(expiryTime);
            return DateTime.Now;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            //var jwtSettings = _config.GetSection("JWT");

            //var token = new JwtSecurityToken(
            //    jwtSettings.GetSection("ValidIssuer").Value,
            //    claims: claims,
            //    expires: GetExpiration(jwtSettings.GetSection("TokenLifeTime").Value),
            //    signingCredentials: signingCredentials
            //);
            //return token;
            return null;
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var secretKey = Encoding.UTF8.GetBytes(_config["JWT:Secret"]);
            //return new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
            return null;
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser usr)
        {
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, usr.UserName)
            //};

            //var roles = await _userManager.GetRolesAsync(usr);
            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            //return claims;

            return null;
        }



        private async Task<bool> ValidateToken(string token)
        {
            //try
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes(_config["JWT:Secret"]);
            //    tokenHandler.ValidateToken(token, new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            //        ClockSkew = TimeSpan.Zero
            //    }, out SecurityToken validatedToken);

            //    var jwtToken = (JwtSecurityToken)validatedToken;
            //    //var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Name").Value);
            //    var username = jwtToken.Claims.First(x => x.Type == "Name").Value;

            //    // find if any user attach to the Token on successful jwt validation
            //    var usr = await _userManager.FindByNameAsync(username);
            //    if (usr == null || usr.Id =="")
            //    {
            //        return false;
            //    }
            //    return true;
            //}
            //catch
            //{
            //    return false;
            //    // do nothing if jwt validation fails
            //    // user is not attached to context so request won't have access to secure routes
            //}

            return false;
        }

        #endregion

    }
}
