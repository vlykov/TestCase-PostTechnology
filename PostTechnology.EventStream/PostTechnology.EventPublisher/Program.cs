using STAN.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PostTechnology.EventPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var clusterId = "test-cluster";
            var clientId = $"producer-{Guid.NewGuid().ToString("N")}";
            var subject = "PostTechnology.EventBus";

            //var options = StanOptions.GetDefaultOptions();
            //options.NatsURL = "nats://localhost:4222";

            using (var cn = new StanConnectionFactory().CreateConnection(clusterId, clientId))
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    var rnd = new Random();

                    while (!cts.IsCancellationRequested)
                    {
                        cn.Publish(subject, BitConverter.GetBytes(rnd.Next(-10, 40)));

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
