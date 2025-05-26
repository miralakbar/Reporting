using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace API.Configurations
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Biosign Biometric KYC",
                    Version = "v1",
                    Description = "Biosign Biometric KYC Services",
                    Contact = new OpenApiContact
                    {
                        Name = "AzInTelecom",
                        Email = "info@azintelecom.az",
                        Url = new Uri("https://azintelecom.az/"),
                    },
                });

                
                c.CustomSchemaIds(a => a.FullName);
            });

            return services;
        }
    }
}
