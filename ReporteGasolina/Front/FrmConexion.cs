using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ReporteGasolina
{
    public partial class FrmConexion : Form
    {
        public string SelectedUsuario { get; private set; }
        public string SelectedCompania { get; private set; }

        // Flag que indica que el usuario debe escoger explícitamente una compañía
        private bool _requireCompanySelection;

        // Logging configurable
        private static readonly object _logLock = new object();
        private static readonly string _logDir;
        private static readonly string _logFile;
        private static readonly bool _loggingEnabled;

        static FrmConexion()
        {
            try
            {
                // Leer flag desde appSettings (EnableDebugLogs = "true" para activar)
                bool enabled = false;
                bool.TryParse(ConfigurationManager.AppSettings["EnableDebugLogs"], out enabled);
                _loggingEnabled = enabled;

                // Obtener directorio de reports desde appSettings (ReportsDirectory)
                var reportsDirSetting = ConfigurationManager.AppSettings["ReportsDirectory"];
                if (string.IsNullOrWhiteSpace(reportsDirSetting))
                {
                    reportsDirSetting = "reports";
                }

                // Si la ruta es relativa, combinar con la base del app domain
                _logDir = Path.IsPathRooted(reportsDirSetting)
                    ? reportsDirSetting
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", reportsDirSetting);

                // Asegurar consistencia del nombre de archivo
                _logFile = Path.Combine(_logDir, "FrmConexion_debug.log");
            }
            catch
            {
                // En caso de cualquier error, deshabilitar logging para no afectar runtime
                _loggingEnabled = false;
                _logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? ".", "reports");
                _logFile = Path.Combine(_logDir, "FrmConexion_debug.log");
            }
        }

        private static void LogDebug(string message)
        {
            if (!_loggingEnabled) return;

            try
            {
                lock (_logLock)
                {
                    Directory.CreateDirectory(_logDir);
                    var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [T:{Thread.CurrentThread.ManagedThreadId}] {message}";
                    File.AppendAllText(_logFile, line + Environment.NewLine);
                    Debug.WriteLine(line);
                }
            }
            catch
            {
                // Nunca lanzar desde logging; si falla, omitimos para no afectar la aplicación.
            }
        }

        public FrmConexion()
        {
            InitializeComponent();
            LogDebug("Ctor: Inicializando FrmConexion");

            // Evitamos que Enter dispare automáticamente el botón Aceptar hasta que corresponda
            AcceptButton = null;

              BtnConectar.Click += BtnConectar_Click;
         //   btnCancelar.Click += (s, e) => Close();

            cmbCompanias.Visible = false;
            lblCompanias.Visible = false;

            // Validar compañías cuando el usuario termine de introducir la contraseña (al perder foco)
            txtPassword.Leave += TxtPassword_Leave;
            // Y también cuando el usuario pulsa Enter en el campo contraseña:
            txtPassword.KeyDown += TxtPassword_KeyDown;

            // Permitir Enter en el combo para confirmar la conexión
            cmbCompanias.KeyDown += CmbCompanias_KeyDown;
            cmbCompanias.GotFocus += (s, e) => AcceptButton = BtnConectar;

            // Cuando el usuario selecciona una compañía, ya no es necesario obligarlo de nuevo
            cmbCompanias.SelectedIndexChanged += CmbCompanias_SelectedIndexChanged;
        }

        private void CmbCompanias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompanias.SelectedIndex >= 0)
            {
                _requireCompanySelection = false;
                AcceptButton = BtnConectar;
                LogDebug($"SelectedIndexChanged: index={cmbCompanias.SelectedIndex}, value={cmbCompanias.SelectedValue}");
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;

                string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
                string password = txtPassword.Text ?? string.Empty;

                LogDebug("TxtPassword_KeyDown: Enter pulsado. usuario=" + usuario);

                if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrEmpty(password))
                {
                    if (!TryLoadCompanias(usuario, password, out string mensaje))
                    {
                        LogDebug("TxtPassword_KeyDown: TryLoadCompanias falló: " + mensaje);
                        MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (cmbCompanias.Visible)
                        {
                            cmbCompanias.Focus();
                            if (!_requireCompanySelection) AcceptButton = BtnConectar;
                            LogDebug("TxtPassword_KeyDown: compañías cargadas. requireSelection=" + _requireCompanySelection);
                        }
                    }
                }
            }
        }

        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Text ?? string.Empty;

            LogDebug("TxtPassword_Leave: usuario=" + usuario);

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(password))
                return;

            if (!TryLoadCompanias(usuario, password, out string mensaje))
            {
                LogDebug("TxtPassword_Leave: TryLoadCompanias falló: " + mensaje);
                MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCompanias.Visible)
            {
                cmbCompanias.Focus();
                if (!_requireCompanySelection) AcceptButton = BtnConectar;
                LogDebug("TxtPassword_Leave: compañías cargadas. requireSelection=" + _requireCompanySelection);
            }
        }

        private void CmbCompanias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                this.BtnConectar.PerformClick();

            }
        }

        private bool TryLoadCompanias(string usuario, string password, out string mensaje)
        {
            mensaje = string.Empty;

            // obtener la parte base (sin credenciales) desde connectionStrings
            var baseConn = ConfigurationManager.ConnectionStrings["AdamDb"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(baseConn))
            {
                MessageBox.Show("Cadena de conexión base no configurada.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // o return según contexto
            }

            SqlConnectionStringBuilder builder;
            try
            {
                builder = new SqlConnectionStringBuilder(baseConn)
                {
                    IntegratedSecurity = false,
                    UserID = usuario,       // desde txtUsuario
                    Password = password     // desde txtPassword (NO registrar)
                };
            }
            catch (Exception ex)
            {
                // no loguear la cadena completa ni la contraseña
                LogDebug("Error al crear SqlConnectionStringBuilder: " + ex.Message);
                mensaje = "Error en la configuración de conexión.";
                return false;
            }
            string connStr = builder.ToString();

            try
            {
                LogDebug("TryLoadCompanias: abriendo conexión. usuario=" + usuario);
                using (SqlConnection cn = new SqlConnection(connStr))
                {
                    cn.Open();

                    const string sqlCompanias = @"
SELECT a.compania, b.nombre_cia
FROM dbo.aut_companias a
JOIN dbo.companias b ON b.compania = a.compania
WHERE a.usuario = @usuario
ORDER BY 2;";

                    var dt = new DataTable();
                    dt.Columns.Add("compania", typeof(string));
                    dt.Columns.Add("nombre_cia", typeof(string));

                    using (var cmd = new SqlCommand(sqlCompanias, cn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                string comp = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0);
                                string nombre = rdr.FieldCount > 1 && !rdr.IsDBNull(1) ? rdr.GetString(1) : comp;
                                dt.Rows.Add(comp, nombre);
                            }
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        mensaje = "El usuario no tiene permiso a ninguna empresa.";
                        // Ocultamos el combo si no hay compañías
                        cmbCompanias.Visible = false;
                        lblCompanias.Visible = false;
                        cmbCompanias.DataSource = null;
                        AcceptButton = null;
                        _requireCompanySelection = false;
                        return false;
                    }

                    // Presentamos el combo para que el usuario confirme la compañía.
                    cmbCompanias.DataSource = dt;
                    cmbCompanias.DisplayMember = "nombre_cia";
                    cmbCompanias.ValueMember = "compania";

                    if (dt.Rows.Count == 1)
                    {
                        // Si solo hay una compañía, seleccionarla automáticamente
                        cmbCompanias.SelectedIndex = 0;
                        _requireCompanySelection = false;
                        AcceptButton = BtnConectar;
                    }
                    else
                    {
                        // Si hay varias, forzar que el usuario seleccione explícitamente
                        cmbCompanias.SelectedIndex = -1;
                        _requireCompanySelection = true;
                        // No asignar AcceptButton hasta que el usuario seleccione
                        AcceptButton = null;
                    }

                    lblCompanias.Visible = true;
                    cmbCompanias.Visible = true;

                    return true;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"No fue posible validar las credenciales o cargar compañías.\r\n\r\n{ex.Message}";
                return false;
            }
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("Ingrese usuario.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Ingrese contraseña.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si el combo no está cargado, intentamos cargar antes de continuar
            if (!cmbCompanias.Visible || cmbCompanias.DataSource == null)
            {
                if (!TryLoadCompanias(usuario, password, out string msgLoad))
                {
                    MessageBox.Show(msgLoad, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Si cargamos varias compañías y pedimos selección explícita, no continuar ahora
                if (_requireCompanySelection && cmbCompanias.SelectedIndex < 0)
                {
                    MessageBox.Show("Seleccione la compañía y vuelva a pulsar Conectar.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Si requerimos selección explícita pero el usuario no ha elegido, detener
            if (_requireCompanySelection && cmbCompanias.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione la compañía y vuelva a pulsar Conectar.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string companiaSeleccionada = null;
            if (cmbCompanias.Visible && cmbCompanias.SelectedValue != null)
            {
                companiaSeleccionada = Convert.ToString(cmbCompanias.SelectedValue);
            }

            if (string.IsNullOrEmpty(companiaSeleccionada))
            {
                MessageBox.Show("Seleccione la compañía y vuelva a pulsar Conectar.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validación final de permisos (nivel de seguridad)
            SqlConnectionStringBuilder builder;
            try
            {
                builder = new SqlConnectionStringBuilder(AppSettings.ConnectionString)
                {
                    IntegratedSecurity = false,
                    UserID = usuario,
                    Password = password
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la cadena de configuración: {ex.Message}", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = builder.ToString();

            try
            {
                using (SqlConnection cn = new SqlConnection(connStr))
                {
                    cn.Open();

                    const string sqlOperacion = @"
SELECT nivel_seguridad
FROM aut_operaciones
WHERE operacion = @operacion
AND usuario = @usuario;";

                    int nivelSeguridad = 0;
                    using (var cmdOp = new SqlCommand(sqlOperacion, cn))
                    {
                        cmdOp.Parameters.AddWithValue("@operacion", AppSettings.Operacion ?? string.Empty);
                        cmdOp.Parameters.AddWithValue("@usuario", usuario);
                        var res = cmdOp.ExecuteScalar();
                        if (res != null && res != DBNull.Value)
                        {
                            int.TryParse(res.ToString(), out nivelSeguridad);
                        }
                    }

                    if (nivelSeguridad == 0)
                    {
                        MessageBox.Show("El usuario no tiene permisos para la operación configurada.", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No fue posible conectar o validar permisos.\r\n\r\n{ex.Message}", "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Guardar la compañía seleccionada en appSettings (intento no crítico)
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["Compania"] != null)
                    config.AppSettings.Settings["Compania"].Value = companiaSeleccionada;
                else
                    config.AppSettings.Settings.Add("Compania", companiaSeleccionada);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }

            // Establecer la cadena de conexión en memoria (no persistir en disco)
            try
            {
                AppSettings.SetRuntimeConnectionString(connStr);
#if DEBUG
                LogDebug("BtnConectar_Click: connection string set in memory (password masked).");
#endif
            }
            catch { /* no crítico */ }

            // Propagar usuario y compañía al AppSettings en memoria para que todos los servicios los usen
            AppSettings.Usuario = usuario;
            AppSettings.Compania = companiaSeleccionada;

            SelectedUsuario = usuario;
            SelectedCompania = companiaSeleccionada;

#if DEBUG
            LogDebug($"BtnConectar_Click: éxito. Usuario={SelectedUsuario}, Compania={SelectedCompania}. Cerrando formulario.");
#endif
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
