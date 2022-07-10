using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowLogFormatter.Configuration
{
    public class AppSettings
    {
        public string StorageAccountName { get; set; }
        public string FlowLogsContainer { get; set; }
        public string BlobNamePrefix { get; set; }
        public string BlobNameSuffix { get; set; }
        public string SASToken { get; set; }
        public string TimeZoneConversion { get; set; }
        public Dictionary<string, string> FlowProtocolNames { get; set; }
        public Dictionary<string, string> FlowDirectionNames { get; set; }
        public Dictionary<string, string> FlowDecisionNames { get; set; }
        public Dictionary<string, string> FlowStateNames { get; set; }
        public string LogOutput { get; set; }

        public AppSettings()
        {
            FlowProtocolNames = new Dictionary<string, string>();
            FlowDirectionNames = new Dictionary<string, string>();
            FlowDecisionNames = new Dictionary<string, string>();
            FlowStateNames = new Dictionary<string, string>();
        }
    }
}
