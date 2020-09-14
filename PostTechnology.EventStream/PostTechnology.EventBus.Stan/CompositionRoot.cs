using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostTechnology.DataAccess.EntityFramework;
using PostTechnology.DataAccess.EntityFramework.Entities;
using PostTechnology.DataAccess.EntityFramework.Repository;
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
                .AddTransient<IMessageRepository<RxMessage>, MessageRepository<RxMessage>>()
                .AddScoped<IMessageRepository<TxMessage>, MessageRepository<TxMessage>>()
                .AddTransient<AppDbContext>()
                .AddScoped<StanConnectionFactory>()
                .AddScoped<IStanConnectionProvider, StanConnectionProvider>()
                .AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .BuildServiceProvider();

            Console.WriteLine("Configuration finished.");
            return container;
        }
    }
}
