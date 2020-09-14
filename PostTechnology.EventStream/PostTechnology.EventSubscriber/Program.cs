using System;
using Microsoft.Extensions.DependencyInjection;
using PostTechnology.DataAccess.EntityFramework;
using PostTechnology.CrossCutting;
using PostTechnology.CrossCutting.Interfaces;

namespace PostTechnology.EventSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("ConsumerService preparing to start.");

            var services = CompositionRoot.ConfigureApp();
            services.AddScoped<ConsumerService>()
                .AddScoped<ITraceMonitor, ConsoleTraceMonitor>();

            var container = services.BuildServiceProvider();
            AppDbContextFactory.PrepareDatabase(container);

            var consumer = container.GetRequiredService<ConsumerService>();
            consumer.Start();

            Console.WriteLine("ConsumerService online.");
            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();

            consumer.Stop();

            container.Dispose();
        }
    }
}
