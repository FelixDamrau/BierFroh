using BierFroh.Modules.KickTipp.Model;
using MudBlazor;

namespace BierFroh.Model;
public class ChartSeriesHelper
{
    public static IEnumerable<ChartSeries> Convert(Season season)
    {
        foreach (var result in season.PlayerResults)
        {
            var data = Enumerable.Range(1, 34)
                .Select(i => result.GetMatchDayPoints(i))
                .Select(r => r.Valid ? (double)r.Value : 0)
                .ToArray();

            for (var i = 1; i < data.Length; ++i)
            {
                data[i] += data[i - 1];
            }
            yield return new ChartSeries { Name = result.PlayerName, Data = data };
        }
    }

    public static IEnumerable<ChartSeries> CleanUp(IEnumerable<ChartSeries> chartSeries)
    {
        foreach (var serie in chartSeries)
        {
            if (serie.Data.Any(d => d != 0))
                yield return serie;
        }
    }
}
