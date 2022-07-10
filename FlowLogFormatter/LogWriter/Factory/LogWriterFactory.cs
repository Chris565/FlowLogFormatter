using FlowLogFormatter.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowLogFormatter.LogWriter.Factory
{
    public abstract class LogWriterFactory
    {
        public abstract ILogWriter Generate(AppSettings settings);
    }

    public class CsvOutputFactory : LogWriterFactory
    {
        public override ILogWriter Generate(AppSettings settings) => new CsvOutput(settings);
    }
}
