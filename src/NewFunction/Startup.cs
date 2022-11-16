using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(NewFunction.Startup))]
namespace NewFunction
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _config;

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            var env = context.EnvironmentName;

            _config = builder.ConfigurationBuilder.Build();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            builder.Services.AddLogging(l =>
            {
                var aiik = _config.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");
                if (!string.IsNullOrEmpty(aiik))
                {
                    l.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => config.ConnectionString = aiik,
                    configureApplicationInsightsLoggerOptions: (options) => { });
                
                }
                l.AddDebug();
                l.AddConsole();
            });
            builder.Services.AddDbContext<ToDoContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }
    }
}
