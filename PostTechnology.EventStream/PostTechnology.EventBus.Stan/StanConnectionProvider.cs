using Microsoft.Extensions.Options;
using PostTechnology.CrossCutting.Config;
using PostTechnology.CrossCutting.Interfaces;
using STAN.Client;

namespace PostTechnology.CrossCutting
{
    public class StanConnectionProvider : IStanConnectionProvider
    {
        private readonly StanConnectionFactory _connectionFactory;
        private readonly IOptions<AppConfig> _appConfig;

        public StanConnectionProvider(StanConnectionFactory connectionFactory, IOptions<AppConfig> appConfig)
        {
            _connectionFactory = connectionFactory;
            _appConfig = appConfig;
        }

        public IStanConnection GetConnection()
        {
            var natsConfig = _appConfig.Value.NatsConnection;

            var options = StanOptions.GetDefaultOptions();
            options.NatsURL = natsConfig.Url;
            return _connectionFactory.CreateConnection(natsConfig.ClusterId, natsConfig.ClientId, options);
        }
    }
}
