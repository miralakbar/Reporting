using Application.Constants;
using Application.Interfaces;
using Application.Repositories.Base;
using Application.Settings;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Infrastructure
{
	public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIoC(this IServiceCollection services,IConfiguration configuration)
        {
            S3AzCloudSettings s3AzCloudSettings = new S3AzCloudSettings();
            s3AzCloudSettings.DefaultBucketName = configuration[CustomEnvironmentVariables.S3AzCloudSettingsDefaultBucketName].Trim();
            s3AzCloudSettings.EndPoint = configuration[CustomEnvironmentVariables.S3AzCloudSettingsEndPoint].Trim();
            s3AzCloudSettings.AccessKey = configuration[CustomEnvironmentVariables.S3AzCloudSettingsAccessKey].Trim();
            s3AzCloudSettings.SecretKey = configuration[CustomEnvironmentVariables.S3AzCloudSettingsSecretKey].Trim();
            configuration.Bind(nameof(S3AzCloudSettings), s3AzCloudSettings);
            services.AddSingleton(s3AzCloudSettings);

            services.AddTransient<IS3AzCloudService, S3AzCloudService>();
            services.AddTransient<ILogger, Logger>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<KycDBContext>(options => options.UseSqlServer(configuration[CustomEnvironmentVariables.ConnectionString].Trim()));

            return services;
        }

		public static IServiceCollection RegisterS3MinioDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			string endpoint = configuration[CustomEnvironmentVariables.S3AzCloudSettingsEndPoint].Trim();
			string accessKey = configuration[CustomEnvironmentVariables.S3AzCloudSettingsAccessKey].Trim();
			string secretKey = configuration[CustomEnvironmentVariables.S3AzCloudSettingsSecretKey].Trim();

			services.AddMinio(minioConfig => minioConfig
			.WithEndpoint(endpoint)
			.WithCredentials(accessKey, secretKey)
			.WithSSL()
			.Build());

			return services;
		}
	}
}