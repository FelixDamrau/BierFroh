using System.Text.RegularExpressions;
using Develix.Essentials.Core;

namespace BierFroh.Modules.Common;
public class DateParser // TODO Das kann man auch einfacher machen, da hier eh angenommen wird, dass der String mit dem Date beginnt. (ParseExact)
{
    private readonly static Regex regex = new(@"^(?<year>\d{4})[-](?<month>\d{2})[-](?<day>\d{2})[\s{1}](?<hour>\d{2})[:](?<minute>\d{2})[:](?<second>\d{2})[,](?<millisecond>\d{3})");

    public static Result<DateTime> GetDate(string rawData)
    {
        if (rawData.Length == 0 || !char.IsDigit(rawData[0]))
            return Result.Fail<DateTime>("Could not parse the date string.");

        var match = regex.Match(rawData);
        if (int.TryParse(match.Groups["day"].Value, out var day)
            && int.TryParse(match.Groups["month"].Value, out var month)
            && int.TryParse(match.Groups["year"].Value, out var year)
            && int.TryParse(match.Groups["hour"].Value, out var hour)
            && int.TryParse(match.Groups["minute"].Value, out var minute)
            && int.TryParse(match.Groups["second"].Value, out var second)
            && int.TryParse(match.Groups["millisecond"].Value, out var millisecond))
        {
            var date = new DateTime(year, month, day, hour, minute, second, millisecond);
            return Result.Ok(date);
        }
        var errorMessage = $"Could not parse the date string: {match.Value}";
        return Result.Fail<DateTime>(errorMessage);
    }
}
