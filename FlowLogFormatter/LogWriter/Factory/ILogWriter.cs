using FlowLogFormatter.Models;
using System.Runtime.InteropServices;

namespace FlowLogFormatter.LogWriter.Factory
{
    public interface ILogWriter
    {
        void ParseContent(List<string> rawContent, [Optional] NameFilter nameFilter);
    }
}
