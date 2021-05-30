using Xunit;

namespace BierFroh.Common.Tests
{
    public class ResultTests
    {
        [Fact]
        public void CreateValidResultIsValid()
        {
            var result = Result<int>.CreateValid(1);

            Assert.True(result.Valid);
        }

        [Fact]
        public void CreateValidResultHasPassedValue()
        {
            var value = 1;

            var result = Result<int>.CreateValid(value);

            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void CreateValidResultHasNoErrorMessage()
        {
            var result = Result<int>.CreateValid(1);

            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void CreateErrorIsNotValid()
        {
            var result = Result<int>.CreateError(string.Empty);

            Assert.False(result.Valid);
        }

        [Fact]
        public void CreateErrorHasPassedErrorMessage()
        {
            var errorMessage = "error";
            var result = Result<int>.CreateError(errorMessage);

            Assert.Equal(errorMessage, result.ErrorMessage);
        }

        [Fact]
        public void CreateErrorHasDefaultValue()
        {
            var result = Result<int>.CreateError(string.Empty);

            Assert.Equal(default, result.Value);
        }
    }
}
