using System.Collections.Generic;
using System.Linq;

namespace KickTippHistory.Tests.Core.Helper
{
    public static class EnumerableHelper
    {
        public static IReadOnlyList<T> TrimTrailingNullValues<T>(IEnumerable<T> collection)
        {
            var list = collection.ToList();
            for (var i = list.Count - 1; i >= 0 && list[i] is null; --i)
            {
                list.RemoveAt(i);
            }
            return list;
        }
    }
}
