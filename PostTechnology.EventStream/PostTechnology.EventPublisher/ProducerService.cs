using PostTechnology.DataAccess.EntityFramework.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using STAN.Client;
using PostTechnology.DataAccess.EntityFramework.Interfaces;
using PostTechnology.CrossCutting.Interfaces;

namespace PostTechnology.EventPublisher
{
    internal sealed class ProducerService
    {
        private IMessageRepository<TxMessage> _repository;
        private IStanConnectionProvider _connectionProvider;
        private IStanConnection _connection;
        private CancellationTokenSource _cancellationTokenSource;
        private ITraceMonitor _monitor;

        public ProducerService(IMessageRepository<TxMessage> repository, IStanConnectionProvider connectionProvider, ITraceMonitor monitor)
        {
            _repository = repository;
            _connectionProvider = connectionProvider;
            _monitor = monitor;
        }

        public void Start()
        {
            var messageNumber = _repository.GetLastMessageNumber();

            _connection = _connectionProvider.GetConnection();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var message = GenerateMessage(++messageNumber);
                    byte[] payload = MessagePackSerializer.Serialize(message);

                    _connection.Publish("PostTechnology.EventBus", payload);

                    _monitor?.Information(message);

                    await _repository.Add(message);
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                }
            }, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _connection.Close();
        }
        
        private TxMessage GenerateMessage(int messageNumber)
        {
            var dbCheckSum = _repository.CalculateCheckSum();
            return new TxMessage { Number = messageNumber, Sent = DateTime.UtcNow, Content = "test", Hash = dbCheckSum.ToString() };
        }
    }
}
