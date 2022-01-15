using BierFroh.Modules.Common;

namespace BierFroh.Modules.Tests.Common;
public class StringExtensionsTest
{
    [Theory]
    [MemberData(nameof(GetValidRequests))]
    public void TestNthOccurance(string? searchString, char value, int nthOccurrence, int expectedIndex)
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

    public static IEnumerable<object?[]> GetValidRequests()
    {
        yield return new object?[] { null, 'c', 1, -1 };
        yield return new object?[] { null, 'd', 4, -1 };
        yield return new object[] { "searchString", "S", 1, 6 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', 1, 4 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', 2, 10 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', 3, 23 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', 4, -1 };
    }

    public static IEnumerable<object?[]> GetInvalidRequests()
    {
        yield return new object?[] { null, 'c', 0 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', 0 };
        yield return new object[] { "Lorem ipsum dolor sit amet", 'm', -1 };
    }
}
