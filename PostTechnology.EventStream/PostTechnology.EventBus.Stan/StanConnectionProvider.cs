using Microsoft.Extensions.Options;
using PostTechnology.EventBus.Stan.Config;
using STAN.Client;

namespace PostTechnology.EventBus.Stan
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
