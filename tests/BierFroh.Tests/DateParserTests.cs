using System;
using System.Collections.Generic;
using BierFroh.Model;
using Xunit;

namespace Tests
{
    public class DateParserTests
    {
        public static IEnumerable<object[]> GetPaseValidDateData()
        {

            yield return new object[] { "2020-03-04 05:06:07,890", new DateTime(2020, 3, 4, 5, 6, 7, 890) };
            yield return new object[] { "2020-03-04 05:06:07,890 Yop!", new DateTime(2020, 3, 4, 5, 6, 7, 890) };
        }

        [Theory]
        [MemberData(nameof(GetPaseValidDateData))]
        public void ParseValidDate(string dateString, DateTime expectedResult)
        {
            var date = DateParser.GetDate(dateString);

            Assert.True(date.Valid);
            Assert.Null(date.ErrorMessage);
            Assert.Equal(expectedResult, date.Value);
        }

        public static IEnumerable<object[]> GetPaseInvalidDateData()
        {
            yield return new object[] { "This is not a date: 2020-03-04 05:06:07,890" };
            yield return new object[] { "2020-03-04 05:06:07,89" };
            yield return new object[] { "Foo" };
        }

        [Theory]
        [MemberData(nameof(GetPaseInvalidDateData))]
        public void ParseInvalidDate(string dateString)
        {
            var date = DateParser.GetDate(dateString);

            Assert.False(date.Valid);
            Assert.NotNull(date.ErrorMessage);
        }
    }
}
