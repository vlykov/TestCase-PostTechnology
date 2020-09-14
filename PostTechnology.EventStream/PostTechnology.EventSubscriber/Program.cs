using PostTechnology.EventBus.Stan;
using STAN.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostTechnology.EventBus.Stan.Config;
using PostTechnology.DataAccess.EntityFramework.Entities;
using PostTechnology.DataAccess.EntityFramework.Repository;
using MessagePack;
using PostTechnology.DataAccess.EntityFramework;

namespace PostTechnology.EventSubscriber
{
    class Program
    {
        static IMessageRepository<RxMessage> _repository;
        static IServiceProvider _services;
        static void Main(string[] args)
        {
            var services = CompositionRoot.ConfigureApp();
            AppDbContextFactory.PrepareDatabase(services);

            _services = services;
            var config = services.GetService<IOptions<AppConfig>>();
            var connectionProvider = services.GetService<IStanConnectionProvider>();

            using (var connection = connectionProvider.GetConnection())
            {
                var cts = new CancellationTokenSource();
                IStanSubscription subscription = null;

                Task.Run(() =>
                {
                    var opts = StanSubscriptionOptions.GetDefaultOptions();
                    opts.DurableName = $"{config.Value.NatsConnection.ClientId}.Durable";

                    subscription = connection.Subscribe("PostTechnology.EventBus", opts, MessageReceived);
                }, cts.Token);

                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                subscription.Close();
                cts.Cancel();
            }
        }

        private static async void MessageReceived(object sender, StanMsgHandlerArgs args)
        {
            var message = ParseMessage(args.Message.Data);
            
            _repository = _services.GetService<IMessageRepository<RxMessage>>();
            await _repository.Add(message);

            Console.WriteLine(message);
        }

        private static RxMessage ParseMessage(byte[] data)
        {
            var message = MessagePackSerializer.Deserialize<TxMessage>(data);
            return new RxMessage { Number = message.Number, Sent = message.Sent, Content = message.Content, Hash = message.Hash, Received = DateTime.UtcNow };
        }
    }
}
