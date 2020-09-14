using PostTechnology.EventBus.Stan;
using STAN.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostTechnology.EventBus.Stan.Config;

namespace PostTechnology.EventSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = CompositionRoot.ConfigureApp();
            var config = services.GetService<IOptions<AppConfig>>();
            var subject = "PostTechnology.EventBus";
            IStanSubscription subscription = null;
            var connectionProvider = services.GetService<IStanConnectionProvider>();

            using (var connection = connectionProvider.GetConnection())
            {
                var cts = new CancellationTokenSource();

                Task.Run(() =>
                {
                    var opts = StanSubscriptionOptions.GetDefaultOptions();
                    opts.DurableName = $"{config.Value.NatsConnection.ClientId}.Durable";

                    subscription = connection.Subscribe(subject, opts, (obj, args) =>
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
