using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

//DI, Serilog, Settings

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup connection to first our configuration 
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            //setup work for our logger (Serilog)
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //DEPENDENCY INJECTION
                    services.AddTransient<IGreetingService, GreetingService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            svc.Run();
        }

        //we wanna log right away 
        //required for serilog
        static void BuildConfig(IConfigurationBuilder builder)
        {
            //sets up our talking to configuration source
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables(); //they can override things in appsettings.json
        }
    }
}
