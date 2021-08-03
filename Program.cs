using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using WeatherChecker.Implementation;
using WeatherChecker.Inteface;

namespace WeatherChecker
{
    class Program
    {
        static  async Task Main(string[] args)
        {
            try
            {
                //establish connection to appsettings.json
                var builder = new ConfigurationBuilder();
                BuildConfig(builder);

                //setup for logger
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

                Log.Logger.Information("Application Starting");

                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddTransient<IWeatherBl, WeatherBl>();
                    })
                    .UseSerilog()
                    .Build();

                var service = ActivatorUtilities.CreateInstance<WeatherBl>(host.Services);

                while (true)
                {
                    Console.Write("Please enter Zip Code: ");
                    string strZipCode = Console.ReadLine();
                    int intZipCode = 0;
                    bool ysnSuccess = int.TryParse(strZipCode, out intZipCode);

                    if (ysnSuccess)
                        await service.GetWeather(intZipCode);
                    else
                        Console.WriteLine("Invalid Zip Code");

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();            
        }
    }
}
