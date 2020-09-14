using STAN.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PostTechnology.EventSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var clusterId = "test-cluster";
            var clientId = $"consumer-AppName";
            var subject = "PostTechnology.EventBus";

            var opts = StanSubscriptionOptions.GetDefaultOptions();
            opts.DurableName = "PostTechnology.Durable";
            IStanSubscription subscription = null;

            //var options = StanOptions.GetDefaultOptions();
            //options.NatsURL = "nats://localhost:4222";

            using (var cn = new StanConnectionFactory().CreateConnection(clusterId, clientId))
            {
                var cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    subscription = cn.Subscribe(subject, opts, (obj, args) =>
                    {
                        var t = BitConverter.ToInt32(args.Message.Data, 0);
                        Console.WriteLine($"{t}C");
                    });
                }, cts.Token);

                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                subscription.Close();
                cts.Cancel();
            }
        }
    }
}
