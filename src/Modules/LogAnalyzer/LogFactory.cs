using System.Text.RegularExpressions;
using BierFroh.Modules.Common;
using Develix.Essentials.Core;

namespace BierFroh.Modules.LogAnalyzer;
public static class LogFactory
{
    private readonly static Regex classRegex = new(@"(?<class>[\w\.]*)\s\[");
    private readonly static Regex methodRegex = new(@"\s\[(?<method>[\w\s]*)\]");
    public static Result<LogEntry> Create(int row, string rawData)
    {
        var date = DateParser.GetDate(rawData);
        if (!date.Valid)
        {
            var errorMessage = $"Failed to parse date. Error: {date.Message}";
            return Result.Fail<LogEntry>(errorMessage);
        }

        var logKind = LogKindIdentifier.GetLogKind(rawData);
        var stringReader = new StringReader(rawData);
        var firstDataLine = stringReader.ReadLine() ?? string.Empty;
        var classMatch = classRegex.Match(firstDataLine);
        var classString = classMatch.Success ? classMatch.Groups["class"].Value : string.Empty;
        var methodMatch = methodRegex.Match(firstDataLine);
        var methodString = methodMatch.Success ? methodMatch.Groups["method"].Value : string.Empty;

        var logEntry = new LogEntry(row, logKind, classString, methodString, rawData, rawData, date.Value);
        return Result.Ok(logEntry);
    }
}
