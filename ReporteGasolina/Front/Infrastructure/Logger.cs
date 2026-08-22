using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ReporteGasolina.Infrastructure
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly bool _enabled;
        private static readonly string _logDir;

        static Logger()
        {
            bool.TryParse(
                ConfigurationManager.AppSettings["EnableDebugLogs"],
                out _enabled);

            _logDir =
                ConfigurationManager.AppSettings["ReportsDirectory"];

            if (string.IsNullOrWhiteSpace(_logDir))
            {
                _logDir = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "reports");
            }
        }

        public static void Debug(
            string source,
            string message)
        {
            if (!_enabled)
                return;

            try
            {
                lock (_lock)
                {
                    Directory.CreateDirectory(_logDir);

                    string file =
                        Path.Combine(
                            _logDir,
                            $"{DateTime.Now:yyyyMMdd}.log");

                    string line =
                        $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}" +
                        $" [T:{Thread.CurrentThread.ManagedThreadId}]" +
                        $" [{source}] {message}";

                    File.AppendAllText(
                        file,
                        line + Environment.NewLine);

                    System.Diagnostics.Debug.WriteLine(line);
                }
            }
            catch
            {
            }
        }
    }
}
