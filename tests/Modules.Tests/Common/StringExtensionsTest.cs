using BierFroh.Modules.Common;

namespace BierFroh.Modules.Tests.Common;
public class StringExtensionsTest
{
    [Theory]
    [MemberData(nameof(GetValidRequests))]
    public void TestNthOccurrence(string? searchString, char value, int nthOccurrence, int expectedIndex)
    {
        var index = searchString.NthIndexOf(value, nthOccurrence);

        Assert.Equal(expectedIndex, index);
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void TestInvalidRequests(string? searchString, char value, int nthOccurrence)
    {
        var exception = Record.Exception(() => searchString.NthIndexOf(value, nthOccurrence));

        Assert.IsType<ArgumentException>(exception);
    }

    public static TheoryData<string?, char, int, int> GetValidRequests()
    {
        return new TheoryData<string?, char, int, int>()
        {
            { null, 'c', 1, -1 },
            { null, 'd', 4, -1 },
            { "searchString", 'S', 1, 6 },
            { "Lorem ipsum dolor sit amet", 'm', 1, 4 },
            { "Lorem ipsum dolor sit amet", 'm', 2, 10 },
            { "Lorem ipsum dolor sit amet", 'm', 3, 23 },
            { "Lorem ipsum dolor sit amet", 'm', 4, -1 },
        };
    }

    public static TheoryData<string?, char, int> GetInvalidRequests()
    {
        return new TheoryData<string?, char, int>()
        {
            { null, 'c', 0 },
            { "Lorem ipsum dolor sit amet", 'm', 0 },
            { "Lorem ipsum dolor sit amet", 'm', -1 },
        };
    }
}
