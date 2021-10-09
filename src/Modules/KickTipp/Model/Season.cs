namespace BierFroh.Modules.KickTipp.Model;
public class Season
{
    public IEnumerable<PlayerSeasonResult> PlayerResults { get; init; } = Enumerable.Empty<PlayerSeasonResult>();
}
