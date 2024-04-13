using BierFroh.Modules.Common;

namespace BierFroh.Modules.Tests.Common;
public class DateParserTests
{
    public static TheoryData<string, DateTime> GetParseValidDateData()
    {
        return new TheoryData<string, DateTime>()
        {
            { "2020-03-04 05:06:07,890", new DateTime(2020, 3, 4, 5, 6, 7, 890) },
            { "2020-03-04 05:06:07,890 Yop!", new DateTime(2020, 3, 4, 5, 6, 7, 890) },
        };
    }

    [Theory]
    [MemberData(nameof(GetParseValidDateData))]
    public void ParseValidDate(string dateString, DateTime expectedResult)
    {
        var date = DateParser.GetDate(dateString);

        Assert.True(date.Valid);
        Assert.Null(date.Message);
        Assert.Equal(expectedResult, date.Value);
    }

    public static TheoryData<string> GetParseInvalidDateData()
    {
        return new TheoryData<string>()
        {
            { "This is not a date: 2020-03-04 05:06:07,890" },
            { "2020-03-04 05:06:07,89" },
            { "Foo" },
        };
    }

    [Theory]
    [MemberData(nameof(GetParseInvalidDateData))]
    public void ParseInvalidDate(string dateString)
    {
        var date = DateParser.GetDate(dateString);

        Assert.False(date.Valid);
        Assert.NotNull(date.Message);
    }
}
