using Microsoft.Extensions.DependencyInjection;
using PostTechnology.CrossCutting;
using PostTechnology.CrossCutting.Interfaces;
using PostTechnology.DataAccess.EntityFramework;
using System;

namespace PostTechnology.EventPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("ProducerService preparing to start.");

            var services = CompositionRoot.ConfigureApp();
            services.AddScoped<ProducerService>()
                .AddScoped<ITraceMonitor, ConsoleTraceMonitor>();

            var container = services.BuildServiceProvider();
            AppDbContextFactory.PrepareDatabase(container);

            var producer = container.GetRequiredService<ProducerService>();
            producer.Start();

            Console.WriteLine("ProducerService online.");
            Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();

            producer.Stop();

            container.Dispose();
        }
    }
}
