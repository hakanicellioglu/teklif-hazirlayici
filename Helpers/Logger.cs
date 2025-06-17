using System;
using System.IO;
using TeklifHazirlayici.Properties;

namespace Teklif_Hazırlayıcı.Helpers
{
    public static class Logger
    {
        private static string GetLogDirectory()
        {
            string dir = Settings.Default.LogDirectory;
            if (string.IsNullOrWhiteSpace(dir))
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dir = Path.Combine(appData, "TeklifHazirlayici", "logs");
            }
            return dir;
        }

        public static void Log(string message)
        {
            try
            {
                var directory = GetLogDirectory();
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, $"{DateTime.Now:yyyyMMdd}.log");
                File.AppendAllText(filePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
            }
            catch
            {
                // I/O hataları göz ardı edilir
            }
        }

        public static void Log(Exception ex)
        {
            Log($"Exception: {ex}");
        }
    }
}
