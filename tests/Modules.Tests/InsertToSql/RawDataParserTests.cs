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
    public void ReadReturnsFalseForEmptyReader()
    {
        var rawDataParser = Create(string.Empty);

        var didRead = rawDataParser.Read();

        Assert.False(didRead);
    }

    [Fact]
    public void ReadReturnsTrueForNonEmptyReader()
    {
        var rawDataParser = Create("Not empty");

        var didRead = rawDataParser.Read();

        Assert.True(didRead);
    }

    [Fact]
    public void ReadReturnsTrueExactlyOnceForSingleLineInput()
    {
        var rawDataParser = Create("Not empty");

        var didReadOnce = rawDataParser.Read();
        var didReadTwice = rawDataParser.Read();

        Assert.True(didReadOnce);
        Assert.False(didReadTwice);
    }

    [Fact]
    public void ReadReturnsTrueExactlyTwiceForTwoLineInput()
    {
        var rawDataParser = Create("Not empty\nLine two");

        var didReadOnce = rawDataParser.Read();
        var didReadTwice = rawDataParser.Read();
        var didReadTrice = rawDataParser.Read();

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
    public void IndexingParserAfterSingleReadsGetsInputLine()
    {
        var line1 = "A line of text";
        var line2 = "Another line of text";
        var rawDataParser = Create(line1 + "\n" + line2);

        _ = rawDataParser.Read();
        var readData = rawDataParser[0];

        Assert.Equal(line1, readData);
    }

    [Fact]
    public void IndexingParserAfterMultipleReadsGetsInputLine()
    {
        var line1 = "A line of text";
        var line2 = "Another line of text";
        var rawDataParser = Create(line1 + "\n" + line2);

        _ = rawDataParser.Read();
        _ = rawDataParser.Read();
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
    public void ParserCountAfterReadReturnsOne()
    {
        var rawDataParser = Create("A text");

        _ = rawDataParser.Read();
        var count = rawDataParser.Count();

        Assert.Equal(1, count);
    }

    [Theory]
    [InlineData("One\tTwo", 2)]
    [InlineData("One\tTwo\tThree", 3)]
    public void ParserCountAfterReadReturnsNumberOfSegments(string rawData, int expectedCount)
    {
        var rawDataParser = Create(rawData);

        _ = rawDataParser.Read();
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
    public void ParserGetLineDataAfterReadGivesSplittedLine()
    {
        var rawDataParser = Create("One\tTwo");
        var expectedLineData = new List<string> { "One", "Two" };

        rawDataParser.Read();
        var lineData = rawDataParser.GetLineData();

        Assert.Equal(expectedLineData, lineData);
    }

    private RawDataParser Create(string rawData)
    {
        var reader = new StringReader(rawData);
        return new RawDataParser(reader);
    }
}
