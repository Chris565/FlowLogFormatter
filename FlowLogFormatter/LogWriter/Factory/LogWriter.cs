using FlowLogFormatter.Configuration;

namespace FlowLogFormatter.LogWriter.Factory
{
    public class LogWriter
    {
        private readonly Dictionary<LogWriterName, LogWriterFactory> _factories;

        public LogWriter()
        {
            _factories = new Dictionary<LogWriterName, LogWriterFactory>();

            foreach (LogWriterName serviceType in Enum.GetValues(typeof(LogWriterName)))
            {
                var factory = (LogWriterFactory)Activator.CreateInstance(Type.GetType("FlowLogFormatter.LogWriter.Factory." + Enum.GetName(typeof(LogWriterName), serviceType) + "OutputFactory"));
                _factories.Add(serviceType, factory);
            }
        }

        public ILogWriter Load(LogWriterName serviceType, AppSettings settings) => _factories[serviceType].Generate(settings);
    }
}
