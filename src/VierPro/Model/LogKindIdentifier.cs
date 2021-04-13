namespace VierPro.Model
{
    public class LogKindIdentifier
    {
        public static LogKind GetLogKind(string rawData)
        {
            return rawData switch
            {
                _ when IsFatal(rawData) => LogKind.Fatal,
                _ when IsError(rawData) => LogKind.Error,
                _ when IsWarning(rawData) => LogKind.Warning,
                _ when IsInfo(rawData) => LogKind.Info,
                _ when IsDebug(rawData) => LogKind.Debug,
                _ => LogKind.Invalid
            };
        }

        private static bool IsFatal(string rawData) => rawData.Contains("FATAL");

        private static bool IsError(string rawData) => rawData.Contains("ERROR");

        private static bool IsWarning(string rawData) => rawData.Contains("WARN");

        private static bool IsInfo(string rawData) => rawData.Contains("INFO");

        private static bool IsDebug(string rawData) => rawData.Contains("DEBUG");
    }
}
