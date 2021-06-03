using System.Globalization;

namespace InsertToSql
{
    public static class SqlValueFormatter
    {
        public static string Parse(string value)
        {
            return value switch
            {
                "" => "''",
                "NULL" => "NULL",
                _ when IsNumeric(value) => value,
                _ => $"'{value}'"
            };
        }

        private static bool IsNumeric(string value)
        {
            return double.TryParse(
                value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var _);
        }
    }
}
