using System;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Application.Settings;
using Application.Constants;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
			   .AddEnvironmentVariables()
			   .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

			var dsn = config[CustomEnvironmentVariables.SentrySettingsDsn];
			var debug = config[CustomEnvironmentVariables.SentrySettingsIsDebugModeEnabled];
			var tracesSampleRate = config[CustomEnvironmentVariables.SentrySettingsTracesSampleRate];
			var attachStacktrace = config[CustomEnvironmentVariables.SentrySettingsAttachStacktrace];
			var includeActivityData = config[CustomEnvironmentVariables.SentrySettingsIncludeActivityData];

			var _sentrySettings = new SentrySettings()
			{
				Dsn = dsn,
				AttachStacktrace = bool.Parse(attachStacktrace),
				IncludeActivityData = bool.Parse(includeActivityData),
				IsDebugModeEnabled = bool.Parse(debug),
				TracesSampleRate = double.Parse(tracesSampleRate)
			};

			try
            {
                Log.Information("************************* Hosting of Web is starting *************************");

                CreateHostBuilder(args, _sentrySettings).Build().Run();

                Log.Information("************************* Hosting of Web has been started *************************");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args, SentrySettings sentrySettings) =>
            Host.CreateDefaultBuilder(args)
			    .ConfigureAppConfiguration((hostingContext, config) =>
			    {
			    	var env = hostingContext.HostingEnvironment;
                
			    	config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
			    		  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
			    		  .AddEnvironmentVariables();
			    })
				.UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSentry(o =>
                    {
                        o.Dsn = sentrySettings.Dsn;
                        o.Debug = sentrySettings.IsDebugModeEnabled;
                        o.TracesSampleRate = sentrySettings.TracesSampleRate;
                        o.AttachStacktrace = sentrySettings.AttachStacktrace;
                        o.IncludeActivityData = sentrySettings.IncludeActivityData;
                        o.MaxRequestBodySize = Sentry.Extensibility.RequestSize.Always;
                    });
                });
    }
}