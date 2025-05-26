using API.Middleware;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Linq;
using Application.Middlewares;
using API.Filters;
using Newtonsoft.Json;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidationFilter));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }).AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});

			services.AddApplicationIoC(_configuration);
            services.AddInfrastructureIoC(_configuration);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(x =>
            //    {
            //        x.SaveToken = true;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[CustomEnvironmentVariables.JWTSettingsKey])),
            //            ValidIssuer = _configuration[CustomEnvironmentVariables.JWTSettingsValidAudience],
            //            ValidAudience = _configuration[CustomEnvironmentVariables.JWTSettingsValidIssuer],
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            RequireExpirationTime = true,
            //            ValidateLifetime = true,
            //        };
            //    });

            services.AddHttpContextAccessor();
			services.AddMemoryCache();
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddSwaggerGen(x =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    x.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"Biosign Biometric KYC: ({_environment.EnvironmentName})",
                        Version = "v1",
                        Description = "Biosign Biometric KYC Services",
                        Contact = new OpenApiContact
                        {
                            Name = "AzInTelecom",
                            Email = "info@azintelecom.az",
                            Url = new Uri("https://azintelecom.az/"),
                        },
                    });
                }

                x.CustomSchemaIds(a => a.FullName);

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                         {
                             Reference= new OpenApiReference()
                             {
                                 Type= ReferenceType.SecurityScheme,
                                 Id ="Bearer"
                             }
                         },
                         Array.Empty<string>()
                    }
                });
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
                //new HeaderApiVersionReader("x-api-version"),
                //new MediaTypeApiVersionReader("x-api-version"));
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

			services.RegisterS3MinioDependencies(_configuration);
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
			app.UseCustomHealthChecks();
			app.UseRouting();
            app.UseMiddleware<SwaggerBasicAuthMiddleware>();
            app.UseSentryTracing();

            app.UseLog();

            app.UseSwagger();

            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(c =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    c.RoutePrefix = string.Empty;
                }
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
