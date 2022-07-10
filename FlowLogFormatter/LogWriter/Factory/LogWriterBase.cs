using FlowLogFormatter.Configuration;
using FlowLogFormatter.Models;
using Newtonsoft.Json.Linq;

namespace FlowLogFormatter.LogWriter.Factory
{
    public abstract class LogWriterBase
    {
        public AppSettings Settings { get; }
        
        public LogWriterBase(AppSettings settings)
        {
            Settings = settings;
        }

        public List<LogRecord> FlattenFlowLog(List<string> rawContent)
        {
            var results = new List<LogRecord>();

            foreach(var nsgEvent in rawContent)
            {
                var rawObj = JObject.Parse(nsgEvent);
                var dataTree = rawObj["records"].ToObject<JArray>();

                foreach(var element in dataTree)
                    results.AddRange(TuplesToRecords(element));
            }
            return results;
        }

        List<LogRecord> TuplesToRecords(JToken flowRecords)
        {
            var results = new List<LogRecord>();

            var utcTime = DateTime.Parse(flowRecords["time"].ToString());
            TimeZoneInfo userZone = TimeZoneInfo.FindSystemTimeZoneById(Settings.TimeZoneConversion);
            var convertedTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, userZone);

            var flows = flowRecords["properties"]["flows"].ToObject<JArray>();
            foreach(var flowItem in flows)
            {
                var ruleName = flowItem["rule"].Value<string>();
                var flowActivity = flowItem.SelectToken("flows[0].flowTuples").ToObject<string[]>();
                foreach(var flowEvent in flowActivity)
                {
                    var eventElements = flowEvent.Split(",");
                    var logRecord = new LogRecord
                    {
                        RuleName = ruleName,
                        TimeStamp = convertedTime,
                        SourceIp = eventElements[1],
                        DestIp = eventElements[2],
                        SourcePort = eventElements[3],
                        DestPort = eventElements[4],
                        Protocol = Settings.FlowProtocolNames[eventElements[5]],
                        Direction = Settings.FlowDirectionNames[eventElements[6]],
                        Decision = Settings.FlowDecisionNames[eventElements[7]],
                        State = Settings.FlowStateNames[eventElements[8]],
                        SourcePacketsSent = !string.IsNullOrEmpty(eventElements[9]) ? int.Parse(eventElements[9]) : 0,
                        SourceBytesSent = !string.IsNullOrEmpty(eventElements[10]) ? int.Parse(eventElements[10]) : 0,
                        DestPacketsRcvd = !string.IsNullOrEmpty(eventElements[11]) ? int.Parse(eventElements[11]) : 0,
                        DestBytesRcvd = !string.IsNullOrEmpty(eventElements[12]) ? int.Parse(eventElements[12]) : 0
                    };
                    results.Add(logRecord);
                }              
            }
            return results;
        }
    }
}
