namespace FlowLogFormatter.Models
{
    public class LogRecord
    {
        public DateTime TimeStamp { get; set; }
        public string RuleName { get; set; }
        public string SourceIp { get; set; }
        public string SourcePort { get; set; }
        public string DestIp { get; set; }
        public string DestPort { get; set; }
        public string? Protocol { get; set; }
        public string? Direction { get; set; }
        public string? Decision { get; set; }
        public string? State { get; set; }
        public int? SourcePacketsSent { get; set; }
        public int? SourceBytesSent { get; set; }
        public int? DestPacketsRcvd { get; set; }
        public int? DestBytesRcvd { get; set; }

        public LogRecord()
        {

        }
    }
}
