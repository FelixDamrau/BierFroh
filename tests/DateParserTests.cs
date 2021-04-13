using System;
using System.Collections.Generic;
using VierPro.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DateParserTests
    {
        public static IEnumerable<object[]> GetPaseValidDateData()
        {

            yield return new object[] { "2020-03-04 05:06:07,890", new DateTime(2020, 3, 4, 5, 6, 7, 890) };
            yield return new object[] { "2020-03-04 05:06:07,890 Yop!", new DateTime(2020, 3, 4, 5, 6, 7, 890) };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPaseValidDateData), DynamicDataSourceType.Method)]
        public void ParseValidDate(string dateString, DateTime expectedResult)
        {
            var date = DateParser.GetDate(dateString);

            Assert.IsTrue(date.Valid);
            Assert.IsNull(date.ErrorMessage);
            Assert.AreEqual(expectedResult, date.Value);
        }

        public static IEnumerable<object[]> GetPaseInvalidDateData()
        {
            yield return new object[] { "This is not a date: 2020-03-04 05:06:07,890" };
            yield return new object[] { "2020-03-04 05:06:07,89" };
            yield return new object[] { "Foo" };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetPaseInvalidDateData), DynamicDataSourceType.Method)]
        public void ParseInvalidDate(string dateString)
        {
            var date = DateParser.GetDate(dateString);

            Assert.IsFalse(date.Valid);
            Assert.IsNotNull(date.ErrorMessage);
        }
    }
}
