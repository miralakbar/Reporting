using Application.Constants;
using Application.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace API.Middleware
{
	public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _hostEnvironment;

		public SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _next = next;
            _configuration = configuration;
			_hostEnvironment = hostEnvironment;
        }
		public async Task InvokeAsync(HttpContext context)
        {
			if ((_hostEnvironment.EnvironmentName.Equals("production", StringComparison.OrdinalIgnoreCase)
				|| _hostEnvironment.EnvironmentName.Equals("staging", StringComparison.OrdinalIgnoreCase))
				&& context.Request.Path.StartsWithSegments("/swagger"))
			{
				var firstSwaggerUsername = _configuration[CustomEnvironmentVariables.SwaggerSecuritySettingsFirstUsername];
				var firstSwaggerPassword = _configuration[CustomEnvironmentVariables.SwaggerSecuritySettingsFirstPassword];

				var secondSwaggerUsername = _configuration[CustomEnvironmentVariables.SwaggerSecuritySettingsSecondUsername];
				var secondSwaggerPassword = _configuration[CustomEnvironmentVariables.SwaggerSecuritySettingsSecondPassword];

				var swaggerSecuritySettings = new List<SwaggerSecurity>()
				{
					new SwaggerSecurity()
					{
						Username = firstSwaggerUsername,
						Password = firstSwaggerPassword
					},
					new SwaggerSecurity()
					{
						Username = secondSwaggerUsername,
						Password = secondSwaggerPassword
					}
				};

				//checking if query parameters are available.
				if (context.Request.Query.TryGetValue("username", out var usernameFromQuery) && context.Request.Query.TryGetValue("password", out var passwordFromQuery))
				{
					foreach (var securitySetting in swaggerSecuritySettings)
					{
						if (usernameFromQuery.Equals(securitySetting.Username) && passwordFromQuery.Equals(securitySetting.Password))
						{
							await _next.Invoke(context).ConfigureAwait(false);
							return;
						}
					}
				}

				//Query params are not available or incorrect. Falling back to the original Basic Auth
				string authHeader = context.Request.Headers["Authorization"];
				if (authHeader != null && authHeader.StartsWith("Basic "))
				{
					// Get the credentials from request header
					var header = AuthenticationHeaderValue.Parse(authHeader);
					var inBytes = Convert.FromBase64String(header.Parameter);
					var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
					var username = credentials[0];
					var password = credentials[1];
					// validate credentials

					foreach (var securitySetting in swaggerSecuritySettings)
					{
						if (username.Equals(securitySetting.Username) && password.Equals(securitySetting.Password))
						{
							await _next.Invoke(context).ConfigureAwait(false);
							return;
						}
					}
				}
				context.Response.Headers["WWW-Authenticate"] = "Basic";
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			}
			else
			{
				await _next.Invoke(context).ConfigureAwait(false);
			}
		}
    }
}
