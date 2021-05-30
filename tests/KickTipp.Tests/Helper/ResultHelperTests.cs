using System;
using BierFroh.Common;
using Xunit;

namespace KickTippHistory.Tests.Core.Helper
{
    public class ResultHelperTests
    {
        [Fact]
        public void ValidResultDoesNothing()
        {
            var validResult = Result<int>.CreateValid(367);

            var exception = Record.Exception(() => ResultHelper.Validate(validResult.Valid, validResult.ErrorMessage));

            Assert.Null(exception);
        }

        [Fact]
        public void NotValidResultThrowsInvalidOperationException()
        {
            var validResult = Result<int>.CreateError("Nope!");

            var exception = Record.Exception(() => ResultHelper.Validate(validResult.Valid, validResult.ErrorMessage));

            Assert.IsType<InvalidOperationException>(exception);
        }
    }
}
