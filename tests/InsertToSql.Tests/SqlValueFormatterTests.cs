using Xunit;

namespace InsertToSql.Tests
{
    public class SqlValueFormatterTests
    {
        [Fact]
        public void EmptyInputReturnsEmptySqlString()
        {
            var result = SqlValueFormatter.Parse(string.Empty);

            Assert.Equal("''", result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData("1234")]
        [InlineData("123.456")]
        [InlineData("-123.567")]
        public void NumericInputInvariantReturn(string numericInputValue)
        {
            var result = SqlValueFormatter.Parse(numericInputValue);

            Assert.Equal(numericInputValue, result);
        }

        [Theory]
        [InlineData("2031-12-12 00:00:00.000")]
        [InlineData("2031-12-12")]
        [InlineData("2031-12")]
        [InlineData("Foo")]
        [InlineData("F08F3672-2AEF-4051-8ABB-469B59AE0249")]
        public void OtherInputReturnsSqlString(string otherInputValue)
        {
            var result = SqlValueFormatter.Parse(otherInputValue);

            Assert.Equal($"'{otherInputValue}'", result);
        }

        [Fact]
        public void NullStringInputInvariantReturn()
        {
            const string nullString = "NULL";

            var result = SqlValueFormatter.Parse(nullString);

            Assert.Equal(nullString, result);
        }
    }
}

