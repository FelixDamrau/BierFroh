using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace KickTippHistory.Tests.Core.Helper
{
    public static class XunitHelper
    {
        public static void AssertTrue([DoesNotReturnIf(false)] bool value) => Assert.True(value);
    }
}
