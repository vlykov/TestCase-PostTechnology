using PostTechnology.CrossCutting.Interfaces;
using System;

namespace PostTechnology.EventPublisher
{
    class ConsoleTraceMonitor : ITraceMonitor
    {
        public void Information(string value)
        {
            Console.WriteLine(value);
        }

        public void Information(object value)
        {
            Console.WriteLine(value);
        }
    }
}
