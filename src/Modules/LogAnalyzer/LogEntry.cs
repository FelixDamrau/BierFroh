namespace BierFroh.Modules.LogAnalyzer;
public record LogEntry(int Row, LogKind LogKind, string Class, string Method, string Message, string RawData, DateTime TimeStamp);

public record LogCollection(string RawData, IReadOnlyList<LogEntry> LogEntries);
