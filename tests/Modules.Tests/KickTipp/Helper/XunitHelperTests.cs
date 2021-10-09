using Xunit.Sdk;

namespace BierFroh.Modules.Tests.KickTipp.Helper;
public class XunitHelperTests
{
    [Fact]
    public void AssertHelperThrowTrueExceptionIfFalseIsPassed()
    {
        static void AssertStatement() => Assert.True(false);

        var exception = Record.Exception(AssertStatement);

        Assert.IsType<TrueException>(exception);
    }

    [Fact]
    public void AssertHelperDoesNothingIfTrueIsPassed()
    {
        static void AssertStatement() => Assert.True(true);

        var exception = Record.Exception(AssertStatement);

        Assert.Null(exception);
    }
}
