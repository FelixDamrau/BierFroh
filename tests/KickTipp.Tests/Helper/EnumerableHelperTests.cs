using System;
using System.Collections.Generic;
using Xunit;

namespace KickTippHistory.Tests.Core.Helper
{
    public class EnumerableHelperTests
    {
        public static IEnumerable<object[]> TrimTrailingNullValuesData()
        {
            yield return new[]
            {
                new int?[] { 1, 2, 3 },
                new int?[] { 1, 2, 3 }
            };

            yield return new[]
            {
                new int?[] { 1, 2, 3, null },
                new int?[] { 1, 2, 3 }
            };

            yield return new[]
            {
                new int?[] { 1, 2, null, 3 },
                new int?[] { 1, 2, null, 3 }
            };


            yield return new[]
            {
                new int?[] { 1, null, 2, 3, null, null, null, null },
                new int?[] { 1, null, 2, 3 }
            };
            yield return new[]
            {
                Array.Empty<int?>(),
                Array.Empty<int?>()
            };

            yield return new[]
            {
                new int?[] { null },
                Array.Empty<int?>()
            };
        }
        [Theory]
        [MemberData(nameof(TrimTrailingNullValuesData))]
        public void TrimTrailingNullValues(IEnumerable<int?> collection, IEnumerable<int?> expectedTrimmedCollection)
        {
            var trimmedCollection = EnumerableHelper.TrimTrailingNullValues(collection);

            Assert.Equal(expectedTrimmedCollection, trimmedCollection);
        }
    }
}
