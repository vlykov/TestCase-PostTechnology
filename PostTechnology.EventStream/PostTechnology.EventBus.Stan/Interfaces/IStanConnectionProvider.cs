using STAN.Client;

namespace PostTechnology.CrossCutting.Interfaces
{
    public interface IStanConnectionProvider
    {
        IStanConnection GetConnection();
    }
}
