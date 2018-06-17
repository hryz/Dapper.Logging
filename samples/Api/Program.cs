using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(GetConfig(args))
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                builder.Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();

        private static IConfiguration GetConfig(string[] args) => 
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

        private static string EnvironmentName => 
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
