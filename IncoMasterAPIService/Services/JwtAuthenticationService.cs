
using HelperClasses;
using IncoMasterAPIService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Services
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        readonly SecureStringConverter _converter;

        public JwtAuthenticationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> JwtAuthenticate(string email, SecureString password)
        {
            var signinKey = Convert.FromBase64String(_config["Jwt:TokenSecret"]);
            var tokenExpirationInMin = Convert.ToDouble(_config["Jwt:TokenExpirationInMin"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "IncoMasterApp",
                IssuedAt = DateTime.UtcNow,
                Audience = null,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpirationInMin),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("Email", email),
                    new Claim("Password", _converter.ConvertToString(password)),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signinKey), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var stoken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
