using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostTechnology.CrossCutting.Config;
using PostTechnology.CrossCutting.Interfaces;
using PostTechnology.DataAccess.EntityFramework;
using PostTechnology.DataAccess.EntityFramework.Entities;
using PostTechnology.DataAccess.EntityFramework.Interfaces;
using PostTechnology.DataAccess.EntityFramework.Repository;
using STAN.Client;

namespace PostTechnology.CrossCutting
{
    public class CompositionRoot
    {
        public static IServiceCollection ConfigureApp()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var services = new ServiceCollection()
                .Configure<AppConfig>(configuration)
                .AddOptions()
                .AddScoped<IMessageRepository<RxMessage>, MessageRepository<RxMessage>>()
                .AddScoped<IMessageRepository<TxMessage>, MessageRepository<TxMessage>>()
                .AddScoped<AppDbContext>()
                .AddScoped<StanConnectionFactory>()
                .AddScoped<IStanConnectionProvider, StanConnectionProvider>()
                .AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
