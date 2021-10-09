namespace BierFroh.Modules.Tests.KickTipp.Helper;
public class TestDataTests
{
    [Fact]
    public void TotalTestDataIsNotEmpty()
    {
        var result = TestData.GetTotal();

        Assert.False(string.IsNullOrWhiteSpace(result));
    }
}
