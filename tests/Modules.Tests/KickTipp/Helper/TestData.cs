namespace BierFroh.Modules.Tests.KickTipp.Helper;
public static class TestData
{
    public static string GetTotal() => File.ReadAllText("./KickTipp/data/total.html");
}
