using System;
using System.Collections.Generic;

namespace BierFroh.Model.BrowserStorage
{
    internal record AppState(DateTime LastVisit, InsertToSqlState InsertToSqlState)
    {
        public static AppState Default = new(
            DateTime.Now,
            new InsertToSqlState(Array.Empty<string>()));
    }

    internal record InsertToSqlState(IEnumerable<string> CachedTableNames);
}
