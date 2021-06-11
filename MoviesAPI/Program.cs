using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration,"Serilog")
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((context,config) =>
                    {
                        config.SetBasePath(context.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                                optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
                    });
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext());
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
