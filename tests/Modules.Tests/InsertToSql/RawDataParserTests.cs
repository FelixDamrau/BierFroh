using BierFroh.Modules.InsertToSql;

namespace BierFroh.Modules.Tests.InsertToSql;
public class RawDataParserTests
{
    [Fact]
    public void NullInputThrowsArgumentNullException()
    {
        static void CreateAction() => _ = new RawDataParser(null!);

        var exception = Record.Exception(CreateAction);

        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task ReadReturnsFalseForEmptyReader()
    {
        var rawDataParser = Create(string.Empty);

        var didRead = await rawDataParser.ReadAsync();

        Assert.False(didRead);
    }

    [Fact]
    public async Task ReadReturnsTrueForNonEmptyReader()
    {
        var rawDataParser = Create("Not empty");

        var didRead = await rawDataParser.ReadAsync();

        Assert.True(didRead);
    }

    [Fact]
    public async Task ReadReturnsTrueExactlyOnceForSingleLineInput()
    {
        var rawDataParser = Create("Not empty");

        var didReadOnce = await rawDataParser.ReadAsync();
        var didReadTwice = await rawDataParser.ReadAsync();

        Assert.True(didReadOnce);
        Assert.False(didReadTwice);
    }

    [Fact]
    public async Task ReadReturnsTrueExactlyTwiceForTwoLineInput()
    {
        var rawDataParser = Create("Not empty\nLine two");

        var didReadOnce = await rawDataParser.ReadAsync();
        var didReadTwice = await rawDataParser.ReadAsync();
        var didReadTrice = await rawDataParser.ReadAsync();

        Assert.True(didReadOnce);
        Assert.True(didReadTwice);
        Assert.False(didReadTrice);
    }

    [Fact]
    public void IndexingParserBeforeReadThrowsException()
    {
        var rawDataParser = Create("Not empty");

        void IndexAction() => _ = rawDataParser[0];
        var exception = Record.Exception(IndexAction);

        Assert.IsType<RawDataParserException>(exception);
    }

    [Fact]
    public async Task IndexingParserAfterSingleReadsGetsInputLine()
    {
        var line1 = "A line of text";
        var line2 = "Another line of text";
        var rawDataParser = Create(line1 + "\n" + line2);

        _ = await rawDataParser.ReadAsync();
        var readData = rawDataParser[0];

        Assert.Equal(line1, readData);
    }

    [Fact]
    public async Task IndexingParserAfterMultipleReadsGetsInputLine()
    {
        var line1 = "A line of text";
        var line2 = "Another line of text";
        var rawDataParser = Create(line1 + "\n" + line2);

        _ = await rawDataParser.ReadAsync();
        _ = await rawDataParser.ReadAsync();
        var readData = rawDataParser[0];

        Assert.Equal(line2, readData);
    }

    [Fact]
    public void ParserCountBeforeReadThrowsException()
    {
        var rawDataParser = Create("A text");

        void CountAction() => _ = rawDataParser.Count();
        var exception = Record.Exception(CountAction);

        Assert.IsType<RawDataParserException>(exception);
    }

    [Fact]
    public async Task ParserCountAfterReadReturnsOne()
    {
        var rawDataParser = Create("A text");

        _ = await rawDataParser.ReadAsync();
        var count = rawDataParser.Count();

        Assert.Equal(1, count);
    }

    [Theory]
    [InlineData("One\tTwo", 2)]
    [InlineData("One\tTwo\tThree", 3)]
    public async Task ParserCountAfterReadReturnsNumberOfSegments(string rawData, int expectedCount)
    {
        var rawDataParser = Create(rawData);

        _ = await rawDataParser.ReadAsync();
        var count = rawDataParser.Count();

        Assert.Equal(expectedCount, count);
    }

    [Fact]
    public void ParserGetLineDataBeforeReadThrowsException()
    {
        var rawDataParser = Create("One\tTwo");

        void CountAction() => _ = rawDataParser.GetLineData();
        var exception = Record.Exception(CountAction);

        Assert.IsType<RawDataParserException>(exception);
    }

    [Fact]
    public async Task ParserGetLineDataAfterReadGivesSplittedLine()
    {
        var rawDataParser = Create("One\tTwo");
        var expectedLineData = new List<string> { "One", "Two" };

        await rawDataParser.ReadAsync();
        var lineData = rawDataParser.GetLineData();

        Assert.Equal(expectedLineData, lineData);
    }

    private static RawDataParser Create(string rawData)
    {
        var reader = new StringReader(rawData);
        return new RawDataParser(reader);
    }
}
