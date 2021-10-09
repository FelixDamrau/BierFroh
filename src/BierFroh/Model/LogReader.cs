using System.Text;
using BierFroh.Modules.Common;
using BierFroh.Modules.LogAnalyzer;
using Microsoft.AspNetCore.Components.Forms;

namespace BierFroh.Model;
public static class LogReader
{
    private const long maxFileSize = 1024 * 1024 * 11; //11MB

    public static async Task<IReadOnlyList<LogEntry>> ReadLogs(IBrowserFile browserFile)
    {
        var lines = await ReadAllLines(browserFile);
        return await Task.Run(() => GetLogEntries(lines));
    }

    private static async Task<IReadOnlyList<string>> ReadAllLines(IBrowserFile browserFile)
    {
        using var stream = browserFile.OpenReadStream(maxFileSize);
        using var reader = new StreamReader(stream);
        var lines = new List<string>();
        var stringBuilder = new StringBuilder();
        while (await reader.ReadLineAsync() is { } line)
        {
            lines.Add(line);
        }
        return lines;
    }

    private static IReadOnlyList<LogEntry> GetLogEntries(IReadOnlyList<string> lines)
    {
        var aggregatedLines = Aggregate(lines);
        var logEntries = new List<LogEntry>();
        foreach (var (row, aggregated) in aggregatedLines)
        {
            var logEntry = LogFactory.Create(row, aggregated);
            if (logEntry.Valid)
                logEntries.Add(logEntry.Value);
        }
        return logEntries;
    }

    private static IEnumerable<(int Row, string Aggregated)> Aggregate(IReadOnlyList<string> lines)
    {
        var stringBuilder = new StringBuilder();
        var lastLogStartRow = -1;
        for (var i = 0; i < lines.Count; ++i)
        {
            var line = lines[i];
            if (IsStartOfRawLogEntry(line))
            {
                if (stringBuilder.Length > 0)
                {
                    yield return (lastLogStartRow, stringBuilder.ToString());
                }
                lastLogStartRow = i + 1;
                stringBuilder.Clear();
                stringBuilder.Append(line);
            }
            else if (stringBuilder.Length > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(line);
            }
        }

        if (stringBuilder.Length > 0)
        {
            yield return (lastLogStartRow, stringBuilder.ToString());
        }
    }

    private static bool IsStartOfRawLogEntry(string line)
    {
        var date = DateParser.GetDate(line);
        return date.Valid;
    }
}
