using System;
using System.Collections.Generic;

namespace BierFroh.Model
{
    public record LogEntry(int Row, LogKind LogKind, string Class, string Method, string Message, string RawData, DateTime TimeStamp);

    public record LogCollection(string RawData, IReadOnlyList<LogEntry> LogEntries);
}
