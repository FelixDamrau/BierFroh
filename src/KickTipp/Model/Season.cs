using System.Collections.Generic;
using System.Linq;

namespace KickTippHistory.Core.Model
{
    public class Season
    {
        public IEnumerable<PlayerSeasonResult> PlayerResults { get; init; } = Enumerable.Empty<PlayerSeasonResult>();
    }
}
