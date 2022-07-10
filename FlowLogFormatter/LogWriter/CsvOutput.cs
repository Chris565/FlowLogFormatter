using FlowLogFormatter.Configuration;
using FlowLogFormatter.LogWriter.Factory;
using FlowLogFormatter.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowLogFormatter.LogWriter
{
    public class CsvOutput : LogWriterBase, ILogWriter
    {
        public CsvOutput(AppSettings settings) : base(settings)
        {

        }
        
        public void ParseContent(List<string> rawContent, NameFilter nameFilter)
        {
            var flattenedLog = FlattenFlowLog(rawContent);

            if(flattenedLog.Any())
            {
                var outputFile = $"{nameFilter.Year}-{nameFilter.Month}-{nameFilter.Day}_sftpFlowLogs.csv";

                using var writer = new StreamWriter(outputFile);
                using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteHeader<LogRecord>();
                csv.NextRecord();
                foreach (var record in flattenedLog)
                {
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }
            }
        }
    }
}
