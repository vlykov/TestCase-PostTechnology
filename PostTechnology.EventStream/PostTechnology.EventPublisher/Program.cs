using PostTechnology.EventBus.Stan;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PostTechnology.DataAccess.EntityFramework;
using PostTechnology.DataAccess.EntityFramework.Repository;
using PostTechnology.DataAccess.EntityFramework.Entities;
using MessagePack;

namespace PostTechnology.EventPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = CompositionRoot.ConfigureApp();
            AppDbContextFactory.PrepareDatabase(services);

            var connectionProvider = services.GetService<IStanConnectionProvider>();
            var repository = services.GetService<IMessageRepository<TxMessage>>();
            var numMessage = repository.GetLastNumber();

            using (var connection = connectionProvider.GetConnection())
            {
                var cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    var rnd = new Random();

                    while (!cts.IsCancellationRequested)
                    {
                        var checksum = repository.CalculateCheckSum();
                        var message = GenerateMessage(++numMessage, checksum);
                        byte[] payload = MessagePackSerializer.Serialize(message);
                        connection.Publish("PostTechnology.EventBus", payload);
                        await repository.Add(message);
                        Console.WriteLine(message);

                        await Task.Delay(1000, cts.Token);
                    }
                }, cts.Token);

                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();
                cts.Cancel();
            }
        }

        static TxMessage GenerateMessage(int number, int checksum)
        {
            return new TxMessage { Number = number, Sent = DateTime.UtcNow, Content = "test", Hash = checksum.ToString() };
        }
    }
}
