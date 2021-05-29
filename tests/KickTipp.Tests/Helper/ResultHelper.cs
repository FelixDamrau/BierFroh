using System;
using System.Diagnostics.CodeAnalysis;

namespace KickTippHistory.Tests.Core
{
    public static class ResultHelper
    {
        public static void Validate([DoesNotReturnIf(false)] bool valid, string? errorMessage)
        {
            if (!valid)
                throw new InvalidOperationException($"The result is not valid! Error message: {errorMessage}");
        }
    }
}
