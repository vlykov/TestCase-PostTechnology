using PostTechnology.DataAccess.EntityFramework.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using STAN.Client;
using Microsoft.Extensions.Options;
using PostTechnology.CrossCutting.Config;
using PostTechnology.DataAccess.EntityFramework.Interfaces;
using PostTechnology.CrossCutting.Interfaces;

namespace PostTechnology.EventSubscriber
{
    internal sealed class ConsumerService
    {
        private IMessageRepository<RxMessage> _repository;
        private IStanConnectionProvider _connectionProvider;
        private IStanConnection _connection;
        private IStanSubscription _subscription;
        private CancellationTokenSource _cancellationTokenSource;
        private ITraceMonitor _monitor;
        private IOptions<AppConfig> _config;

        private AutoResetEvent _storingData = new AutoResetEvent(true);

        public ConsumerService(IMessageRepository<RxMessage> repository, IStanConnectionProvider connectionProvider, IOptions<AppConfig> config, ITraceMonitor monitor)
        {
            _repository = repository;
            _connectionProvider = connectionProvider;
            _config = config;
            _monitor = monitor;            
        }

        public void Start()
        {
            _connection = _connectionProvider.GetConnection();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                var opts = StanSubscriptionOptions.GetDefaultOptions();
                opts.DurableName = $"{_config.Value.NatsConnection.ClientId}.Durable";

                _subscription = _connection.Subscribe("PostTechnology.EventBus", opts, MessageReceived);
            }, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _subscription.Close();
            _connection.Close();
            _storingData.Close();
        }

        private async void MessageReceived(object sender, StanMsgHandlerArgs args)
        {
            var message = ParseMessage(args.Message.Data);

            _monitor?.Information(message);

            _storingData.WaitOne(); //из разных потоков нельзя обращаться к одному DbContext - синхронизируем
            await _repository.Add(message);
            _storingData.Set();
        }

        private RxMessage ParseMessage(byte[] data)
        {
            var message = MessagePackSerializer.Deserialize<TxMessage>(data);
            return new RxMessage { Number = message.Number, Sent = message.Sent, Content = message.Content, Hash = message.Hash, Received = DateTime.UtcNow };
        }
    }
}
