namespace InsertToSql
{
    public static class SqlValueFormatter
    {
        public static string Parse(string value)
        {
            return value switch
            {
                "NULL" => "NULL",
                _ => $"'{value}'"
            };
        }
    }
}
