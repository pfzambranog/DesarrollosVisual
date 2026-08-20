using ReporteGasolina.Infrastructure;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ReporteGasolina
{
    public partial class FrmConexion : Form
    {
        public string SelectedUsuario { get; private set; }
        public string SelectedCompania { get; private set; }

        // Flag que indica que el usuario debe escoger explícitamente una compañía
        private bool _requireCompanySelection;

        public FrmConexion()
        {
            InitializeComponent();
            // Evitamos que Enter dispare automáticamente el botón Aceptar hasta que corresponda
            AcceptButton = null;

            BtnConectar.Click += BtnConectar_Click;
            btnCancelar.Click += (s, e) => Close();
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
            // Si el usuario selecciona una compañía, quitamos la obligación de confirmarla
            if (cmbCompanias.SelectedIndex >= 0)
            {
                _requireCompanySelection = false;
                AcceptButton = BtnConectar;
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Suprimimos la pulsación de Enter para que no active ningún botón.
                e.SuppressKeyPress = true;
                e.Handled = true;

                // Ejecutamos la misma lógica que al salir del control: cargar compañías.
                // Pero NO mostramos MessageBox informativo aquí (mejor UX).
                string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
                string password = txtPassword.Text ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrEmpty(password))
                {
                    if (!TryLoadCompanias(usuario, password, out string mensaje))
                    {
                        MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Si se cargaron compañías, ponemos foco en el combo y permitimos Enter para conectar.
                        if (cmbCompanias.Visible)
                        {
                            cmbCompanias.Focus();
                            // Si hay que seleccionar explícitamente, AcceptButton queda asignado
                            // cuando el usuario elija (ver SelectedIndexChanged).
                            if (!_requireCompanySelection) AcceptButton = BtnConectar;
                        }
                    }
                }
            }
        }

        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            // Mantener comportamiento similar a KeyDown pero sin forzar MessageBox informativo.
            string usuario = txtUsuario.Text?.Trim() ?? string.Empty;
            string password = txtPassword.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(password))
                return;

            if (!TryLoadCompanias(usuario, password, out string mensaje))
            {
                MessageBox.Show(mensaje, "Conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCompanias.Visible)
            {
                // Colocar foco en el combo; AcceptButton se activa cuando haya selección
                cmbCompanias.Focus();
                if (!_requireCompanySelection) AcceptButton = BtnConectar;
            }
        }

        private void CmbCompanias_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                // Simular clic en conectar (se reusar validaciones existentes)
                BtnConectar.PerformClick();
            }
        }

        /// <summary>
        /// Intenta conectar con las credenciales y cargar las compañías del usuario en el combo.
        /// No realiza el cierre del formulario: deja al usuario pulsar Conectar.
        /// </summary>
        private bool TryLoadCompanias(string usuario, string password, out string mensaje)
        {
            mensaje = string.Empty;
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
                mensaje = $"Error en la cadena de configuración: {ex.Message}";
                return false;
            }

            string connStr = builder.ToString();

            try
            {
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

            SelectedUsuario = usuario;
            SelectedCompania = companiaSeleccionada;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
