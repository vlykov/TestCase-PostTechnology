namespace PostTechnology.CrossCutting.Interfaces
{
    public interface ITraceMonitor
    {
        void Information(string value);
        void Information(object value);
    }
}
