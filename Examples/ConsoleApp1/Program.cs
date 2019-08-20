using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Libraries.ConsulHelper.Models;
using Rocket.Libraries.ConsulHelper.Services;
using Rocket.Libraries.ConsulHelper.Services.ConsulRegistryReading;
using Rocket.Libraries.ConsulHelper.Services.ConsulRegistryWriting;
using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static string _otherAppName;

        private const string ThisAppName = "ConsulHelper.Example.App1";

        private static IServiceProvider _serviceProvider;

        private static ILogger<Program> _logger;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    var configuration = GetConfiguration();
                    _serviceProvider = new ServiceCollection()
                        .AddLogging()
                        .AddHttpClient()
                        .Configure<ConsulRegistrationSettings>(configuration.GetSection("ConsulSettings"))
                        .AddSingleton<IConsulRegistryReader, ConsulRegistryReader>()
                        .AddSingleton<IConsulRegistryWriter, ConsulRegistryWriter>()
                        .BuildServiceProvider();
                }
                return _serviceProvider;
            }
        }

        internal static ILogger<Program> Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = ServiceProvider.GetService<ILoggerFactory>()
                        .AddConsole()
                        .CreateLogger<Program>();
                }
                return _logger;
            }
        }

        private static void Main(string[] args)
        {
            _otherAppName = GetInput($"Enter name of service you wish to lookup on Consul (tip: you can enter '{ThisAppName}' if you have no other services registered)");
            var registrar = ServiceProvider.GetService<IConsulRegistryWriter>();
            registrar.RegisterAsync().GetAwaiter().GetResult();
            Logger.LogInformation($"Starting. Will register this service as '{ThisAppName}'");
            Logger.LogInformation($"Will ping Consul at 10 second intervals until address for '{_otherAppName}' is obtained");
            Logger.LogInformation($"You may hit Enter at anytime to quit");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var timer = new Timer(PingConsul, null, 0, 100000);
            Console.ReadLine();
        }

        private static void PingConsul(object obj)
        {
            var consulRegistryReader = ServiceProvider.GetService<IConsulRegistryReader>();
            var timer = new System.Timers.Timer(10000);
            var otherAppBaseUrl = consulRegistryReader.GetServiceBaseAddressAsync(_otherAppName)
                .GetAwaiter()
                .GetResult();

            var foundBaseUrl = !string.IsNullOrEmpty(otherAppBaseUrl);
            if (foundBaseUrl)
            {
                Logger.LogInformation($"Succeeded {_otherAppName} baseUrl is {otherAppBaseUrl}");
                timer.Stop();
            }
            else
            {
                Logger.LogInformation($"Didn't find base url of {_otherAppName}");
            }
            timer.Start();
        }

        private static string GetInput(string question)
        {
            Console.Write($"{question}: ");
            return Console.ReadLine();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var appSettingsGlobal = "appsettings.json";
            var builder = new ConfigurationBuilder()
                .AddJsonFile(appSettingsGlobal, optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}