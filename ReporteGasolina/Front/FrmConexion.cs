using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using RepGas.Infrastructure;
     

namespace RepGas
{
    public partial class FrmConexion : Form
    {
        public string SelectedUsuario { get; private set; }
        public string SelectedCompania { get; private set; }

        // Flag que indica que el usuario debe escoger explícitamente una compañía
        private bool _requireCompanySelection;
        private bool _loadingCompanies;

        public FrmConexion()
        {
            InitializeComponent();

            RepGas.Infrastructure.Logger.Debug(nameof(FrmConexion), "Ctor: Inicializando FrmConexion");


            // Evitamos que Enter dispare automáticamente el botón Aceptar hasta que corresponda
            AcceptButton = null;

              BtnConectar.Click += BtnConectar_Click;
         //   btnCancelar.Click += (s, e) => Close();

            cmbCompanias.Visible = false;
            lblCompanias.Visible = false;

            // Validar compañías cuando el usuario termine de introducir la contraseña (al perder foco)
            // txtPassword.Leave += TxtPassword_Leave;

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
                Logger.Debug(nameof(FrmConexion), $"SelectedIndexChanged: index={cmbCompanias.SelectedIndex}");

            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {

            Logger.Debug(nameof(FrmConexion), $"EVENTO: TxtPassword_KeyDown Key={e.KeyCode}");

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;


                string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
                string password = txtPassword.Text ?? string.Empty;

                Logger.Debug(nameof(FrmConexion), "TxtPassword_KeyDown: Enter pulsado. usuario=" + usuario);

                if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrEmpty(password))
                {
                    if (!TryLoadCompanias(usuario, password, out string mensaje))
                    {
                        Logger.Debug(nameof(FrmConexion), "TxtPassword_KeyDown: TryLoadCompanias falló: " + mensaje);
                        MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (cmbCompanias.Visible)
                        {
                            cmbCompanias.Focus();
                            if (!_requireCompanySelection) AcceptButton = BtnConectar;
                            Logger.Debug(nameof(FrmConexion), "TxtPassword_KeyDown: compañías cargadas. requireSelection=" + _requireCompanySelection);

                        }
                    }
                }
            }
        }

        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Text ?? string.Empty;

            Logger.Debug(nameof(FrmConexion), "EVENTO TxtPassword_Leave");

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(password))

            return;


            if (!TryLoadCompanias(usuario, password, out string mensaje))
            {
                Logger.Debug(nameof(FrmConexion), "TxtPassword_Leave: TryLoadCompanias falló: " + mensaje);

                MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCompanias.Visible)
            {
                cmbCompanias.Focus();
                if (!_requireCompanySelection) AcceptButton = BtnConectar;
                Logger.Debug(nameof(FrmConexion), "TxtPassword_Leave: compañías cargadas. requireSelection=" + _requireCompanySelection);
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

        private bool TryLoadCompanias(
           string usuario,
           string password,
           out string mensaje)
        {
            mensaje = string.Empty;

            var baseConn =
                ConfigurationManager
                    .ConnectionStrings["AdamDb"]?
                    .ConnectionString;

            if (string.IsNullOrWhiteSpace(baseConn))
            {
                MessageBox.Show(
                    "Cadena de conexión base no configurada.",
                    "Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            SqlConnectionStringBuilder builder;

            try
            {
                builder =
                    new SqlConnectionStringBuilder(baseConn)
                    {
                        IntegratedSecurity = false,
                        UserID = usuario,
                        Password = password
                    };
            }
            catch (Exception ex)
            {
                Logger.Debug(
                    nameof(FrmConexion),
                    "Error al crear SqlConnectionStringBuilder: "
                    + ex.Message);

                mensaje =
                    "Error en la configuración de conexión.";

                return false;
            }

            string connStr = builder.ToString();

            try
            {
                Logger.Debug(
                    nameof(FrmConexion),
                    "TryLoadCompanias: abriendo conexión");

                const string sqlCompanias = @"SELECT a.compania, b.nombre_cia
                                              FROM   dbo.aut_companias a
                                              INNER  JOIN dbo.companias b
                                              ON     b.compania = a.compania
                                              WHERE a.usuario = @usuario
                                              ORDER BY 2;";

                _loadingCompanies = true;

                SqlHelper sqlHelper =new SqlHelper(connStr);

                DataTable dt =
                    sqlHelper.Execute(
                        sqlCompanias,
                        CommandType.Text,
                        new SqlParameter(
                            "@usuario",
                            usuario));

                Logger.Debug(nameof(FrmConexion), $"Companias={dt.Rows.Count}");

                if (dt.Rows.Count == 0)
                {
                    mensaje =
                        "El usuario no tiene permiso a ninguna empresa.";

                    cmbCompanias.Visible = false;
                    lblCompanias.Visible = false;
                    cmbCompanias.DataSource = null;

                    AcceptButton = null;

                    _requireCompanySelection = false;

                    return false;
                }

                cmbCompanias.DataSource = null;

                cmbCompanias.DisplayMember = "nombre_cia";
                cmbCompanias.ValueMember = "compania";

                cmbCompanias.DataSource = dt;

                cmbCompanias.SelectedIndex = -1;
                cmbCompanias.Text = string.Empty;


                if (dt.Rows.Count == 1)
                {
                    cmbCompanias.SelectedIndex = 0;

                    _requireCompanySelection = false;

                    AcceptButton = BtnConectar;
                }
                else
                {
                    cmbCompanias.SelectedIndex = -1;

                    _requireCompanySelection = true;

                    AcceptButton = null;
                }

                lblCompanias.Visible = true;
                cmbCompanias.Visible = true;

                _loadingCompanies = false;

                return true;

            }
            catch (Exception ex)
            {
                Logger.Debug(
                    nameof(FrmConexion),
                    $"TryLoadCompanias ERROR. Usuario={usuario}. {ex.Message}");

                mensaje =
                    $"No fue posible validar las credenciales o cargar compañías.\r\n\r\n{ex.Message}";

                return false;
            }

        }


        private void BtnConectar_Click(object sender, EventArgs e)
        {


            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Text ?? string.Empty;

            Logger.Debug(nameof(FrmConexion),  "EVENTO BtnConectar_Click");

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
                const string sqlOperacion = @"
SELECT nivel_seguridad
FROM aut_operaciones
WHERE operacion = @operacion
AND usuario = @usuario;";

                SqlHelper sqlHelper =
                    new SqlHelper(connStr);

                object result =
                    sqlHelper.ExecuteScalar(
                        sqlOperacion,
                        CommandType.Text,
                        new SqlParameter(
                            "@operacion",
                            AppSettings.Operacion ?? string.Empty),
                        new SqlParameter(
                            "@usuario",
                            usuario));

                int nivelSeguridad = 0;

                if (result != null &&
                    result != DBNull.Value)
                {
                    int.TryParse(
                        result.ToString(),
                        out nivelSeguridad);
                }

                if (nivelSeguridad == 0)
                {
                    MessageBox.Show(
                        "El usuario no tiene permisos para la operación configurada.",
                        "Conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(
                    nameof(FrmConexion),
                    $"Validación de permisos ERROR. Usuario={usuario}. {ex.Message}");

                MessageBox.Show(
                    $"No fue posible conectar o validar permisos.\r\n\r\n{ex.Message}",
                    "Conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

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
                Logger.Debug(nameof(FrmConexion), "Connection string establecida en memoria");
            }
            catch { /* no crítico */ }

            // Propagar usuario y compañía al AppSettings en memoria para que todos los servicios los usen
            AppSettings.Usuario = usuario;
            AppSettings.Compania = companiaSeleccionada;

            SelectedUsuario = usuario;
            SelectedCompania = companiaSeleccionada;

#if DEBUG
            Logger.Debug(nameof(FrmConexion), $"Login exitoso. Usuario={SelectedUsuario}, Compania={SelectedCompania}");

#endif
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmConexion_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}

