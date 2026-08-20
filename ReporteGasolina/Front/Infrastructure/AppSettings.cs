using System.Configuration;

namespace ReporteGasolina
{
    public static class AppSettings
    {
        private static readonly object _sync = new object();

        static AppSettings()
        {
            _compania = ConfigurationManager.AppSettings["Compania"] ?? string.Empty;
            _operacion = ConfigurationManager.AppSettings["Operacion"] ?? string.Empty;
            _usuario = ConfigurationManager.AppSettings["Usuario"] ?? string.Empty;
        }

        private static string _compania;
        public static string Compania
        {
            get { lock (_sync) { return _compania; } }
            set { lock (_sync) { _compania = value; } }
        }

        private static string _operacion;
        public static string Operacion
        {
            get { lock (_sync) { return _operacion; } }
            set { lock (_sync) { _operacion = value; } }
        }

        private static string _usuario;
        public static string Usuario
        {
            get { lock (_sync) { return _usuario; } }
            set { lock (_sync) { _usuario = value; } }
        }

        // override en memoria para la connection string (no se persiste en disco)
        private static string _overrideConnectionString;

        public static void SetRuntimeConnectionString(string connectionString)
        {
            lock (_sync)
            {
                _overrideConnectionString = connectionString;
            }
        }

        // Exponer ConnectionString: si hay override en memoria, usarlo; si no, leer del config
        public static string ConnectionString
        {
            get
            {
                lock (_sync)
                {
                    if (!string.IsNullOrEmpty(_overrideConnectionString))
                        return _overrideConnectionString;

                    return ConfigurationManager.ConnectionStrings["AdamDb"]?.ConnectionString ?? string.Empty;
                }
            }
        }

        public static string GetConnectionString(string name = "AdamDb")
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString ?? string.Empty;
        }
    }
}
