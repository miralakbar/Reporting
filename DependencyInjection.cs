using Application.AuthSettings;
using Application.HealthChecks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Application
{
	public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationIoC(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = new JWTSettings();
            configuration.Bind(nameof(JWTSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddHealthChecks()
				.AddCheck<ApplicationHealthChecks>("Application")
				.AddCheck<DbHealthChecks>("Database");
			return services;
        }

		public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseHealthChecks("/health", new HealthCheckOptions()
			{
				ResponseWriter = async (context, report) =>
				{
					context.Response.ContentType = "application/json";

					var response = new HealthCheckResponse()
					{
						Status = report.Status.ToString(),
						Checks = report.Entries.Select(x => new HealthCheck()
						{
							Component = x.Key,
							Description = x.Value.Description,
							Status = x.Value.Status.ToString(),
						}),
						TotalCheckDuration = report.TotalDuration
					};

					await context.Response.WriteAsync(JsonSerializer.Serialize(response));
				}
			});

			return applicationBuilder;
		}

	}
}
