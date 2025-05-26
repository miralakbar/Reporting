using Application.AuthSettings;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Filters
{
    public class Auth : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);

            var fullToken = value.ToString();
            if (string.IsNullOrEmpty(fullToken))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnAuthActionResult();
                return;
            }

            var config = context.HttpContext.RequestServices.GetService<JWTSettings>();
            int startIndex = fullToken.IndexOf(' ');
            string tokenOnly = fullToken.Substring(startIndex, fullToken.Length - startIndex).Trim();

            var claims = GetPrincipalFromToken(tokenOnly, new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
                ValidIssuer = config.ValidIssuer,
                ValidAudiences = new List<string> { config.ValidAudience },
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = false
            });

            if (claims == null)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new UnAuthActionResult();
                return;
            }
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch { return null; }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        =>
            (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                StringComparison.InvariantCultureIgnoreCase);
    }
}
