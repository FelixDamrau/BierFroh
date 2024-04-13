namespace BierFroh.Modules.InsertToSql;
public class RawDataParser
{
    private readonly TextReader reader;
    private IReadOnlyList<string>? currentSplittedLine;

    public RawDataParser(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        this.reader = reader;
    }

    public string this[int i]
    {
        get => currentSplittedLine?[i] ?? throw new RawDataParserException();
    }

    public int Count()
    {
        return currentSplittedLine is not null 
            ? currentSplittedLine.Count 
            : throw new RawDataParserException();
    }

    public IList<string> GetLineData()
    {
        return currentSplittedLine is not null 
            ? [.. currentSplittedLine]
            : throw new RawDataParserException();
    }

    public async Task<bool> ReadAsync()
        {
        if (await reader.ReadLineAsync() is not string readLine)
            return false;
        }

        var splitted = readLine.Split("\t");
        currentSplittedLine = splitted;
        return true;
    }
}
