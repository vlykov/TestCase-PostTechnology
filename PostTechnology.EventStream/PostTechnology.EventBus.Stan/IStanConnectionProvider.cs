using STAN.Client;

namespace PostTechnology.EventBus.Stan
{
    public interface IStanConnectionProvider
    {
        IStanConnection GetConnection();
    }
}
