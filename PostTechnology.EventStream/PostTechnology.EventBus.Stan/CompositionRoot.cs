using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostTechnology.EventBus.Stan.Config;
using STAN.Client;
using System;

namespace PostTechnology.EventBus.Stan
{
    public class CompositionRoot
    {
        public static IServiceProvider ConfigureApp()
        {
            Console.WriteLine("Started Configuration...");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            Console.WriteLine("Started Services Configuration...");
            var container = new ServiceCollection()
                .Configure<AppConfig>(configuration)
                .AddOptions()
                .AddScoped<StanConnectionFactory>()
                .AddScoped<IStanConnectionProvider, StanConnectionProvider>()
                .BuildServiceProvider();

            Console.WriteLine("Configuration finished.");
            return container;
        }
    }
}
