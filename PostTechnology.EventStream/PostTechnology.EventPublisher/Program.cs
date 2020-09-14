using PostTechnology.EventBus.Stan;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PostTechnology.EventPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = CompositionRoot.ConfigureApp();
            var connectionProvider = services.GetService<IStanConnectionProvider>();
            var subject = "PostTechnology.EventBus";

            using (var connection = connectionProvider.GetConnection())
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    var rnd = new Random();

                    while (!cts.IsCancellationRequested)
                    {
                        connection.Publish(subject, BitConverter.GetBytes(rnd.Next(-10, 40)));

                        await Task.Delay(1000, cts.Token);
                    }
                }, cts.Token);

                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                cts.Cancel();
            }
        }
    }
}
