using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace BierFroh.Modules.Tests.KickTipp.Helper
{
    public static class XunitHelper
    {
        public static void AssertTrue([DoesNotReturnIf(false)] bool value) => Assert.True(value);
    }
}
