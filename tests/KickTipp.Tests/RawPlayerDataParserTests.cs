using System.IO;
using System.Linq;
using KickTippHistory.Core;
using KickTippHistory.Tests.Core.Helper;
using Xunit;

namespace KickTippHistory.Tests.Core
{
    public class RawPlayerDataParserTests
    {
        [Fact]
        public async void TestDataReturns14RawPlayerData()
        {
            var data = ReadTestData();

            var playerData = await RawPlayerDataParser.GetAllRawPlayerData(data);

            XunitHelper.AssertTrue(playerData.Valid);
            Assert.Equal(14, playerData.Value.Count());
        }

        [Fact]
        public async void EmptyTestDataReturnsError()
        {
            var data = string.Empty;

            var playerData = await RawPlayerDataParser.GetAllRawPlayerData(data);

            Assert.False(playerData.Valid);
        }

        private static string ReadTestData()
        {
            return File.ReadAllText("./data/total.html");
        }
    }
}
