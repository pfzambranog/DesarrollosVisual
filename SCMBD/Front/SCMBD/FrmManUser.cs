using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmManUser : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _operacion; 
        private readonly string _cadenaConexion;
        private readonly int _idTipoUsuarioAct;

        private DataGridView dgv;

        // ✅ Constructor con los 5 parámetros EN ORDEN
        public FrmManUser(int idUsuario, string claveUsuario, DataTable dtPermisos, string operacion, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _operacion = operacion; 
            _cadenaConexion = cadenaConexion;
            _idTipoUsuarioAct = ObtenerTipoUsuario(_idUsuario);

            InitializeComponent();
        }

        private void FrmManUser_Load(object sender, EventArgs e)
        {
            CargarLogo();
            txtOperacion.Text = _operacion;  
            txtUsuario.Text = _claveUsuario;
            CargarEstatus();
            CargarTipoUsuario();
            CargarGridUsuarios();

            bool esAdmin = (_idTipoUsuarioAct == 1);
            TextPassw.Enabled = esAdmin;
            BtnProcesar.Enabled = esAdmin;
        }

        private int ObtenerTipoUsuario(int idUsuario)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT idTipoUsuario " +
                    "FROM dbo.segUsuariosTbl " +
                    "WHERE idUsuario = @IdUsuario", cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cn.Open();
                    var resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
            catch { return 0; }
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void CargarEstatus()
        {
            cmbEstatus.Items.Clear();
            cmbEstatus.DisplayMember = "Descripcion";
            cmbEstatus.ValueMember = "Valor";
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, descripcion FROM dbo.catGeneralesTbl " +
                    "WHERE tabla = 'segUsuariosTbl' AND columna = 'idEstatus' ORDER BY valor", cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        cmbEstatus.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Estatus: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTipoUsuario()
        {
            cmbIdTipoUsuario.Items.Clear();
            cmbIdTipoUsuario.DisplayMember = "Descripcion";
            cmbIdTipoUsuario.ValueMember = "Valor";
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, descripcion FROM dbo.catGeneralesTbl " +
                    "WHERE tabla = 'segUsuariosTbl' AND columna = 'idTipoUsuario' ORDER BY valor", cn))
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        cmbIdTipoUsuario.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Tipo Usuario: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGridUsuarios()
        {
            try
            {
                string sql = @"
                SELECT 
                    a.idUsuario, 
                    a.claveUsuario, 
                    ISNULL(a.nombres, CHAR(32)) AS Nombres, 
                    a.primerApellido, 
                    ISNULL(a.segundoApellido, CHAR(32)) AS segundoApellido,
                    a.correo,
                    b.descripcion AS estatus, 
                    c.descripcion AS tipoUsuario
                FROM dbo.segUsuariosTbl a
                INNER JOIN dbo.catGeneralesTbl b 
                    ON b.tabla = 'segUsuariosTbl' 
                   AND b.columna = 'idEstatus' 
                   AND b.valor = a.idEstatus
                INNER JOIN dbo.catGeneralesTbl c 
                    ON c.tabla = 'segUsuariosTbl' 
                   AND c.columna = 'idTipoUsuario' 
                   AND c.valor = a.idTipoUsuario
                ORDER BY a.idUsuario";

                DataTable _dtUsuarios = new DataTable();
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(_dtUsuarios);
                }

                // ✅ Crear y configurar Grid
                dgv = new DataGridView
                {
                    BackgroundColor = Color.LightSteelBlue,
                    GridColor = Color.SteelBlue,
                    BorderStyle = BorderStyle.Fixed3D,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    ReadOnly = true,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    EnableHeadersVisualStyles = false
                };
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                dgv.DataSource = _dtUsuarios;

                // ✅ Encabezados y anchos
                dgv.DataBindingComplete += (s, e) =>
                {
                    dgv.Columns["idUsuario"].HeaderText = "ID";
                    dgv.Columns["claveUsuario"].HeaderText = "USUARIO";
                    dgv.Columns["Nombres"].HeaderText = "NOMBRES";
                    dgv.Columns["primerApellido"].HeaderText = "PRIMER APELLIDO";
                    dgv.Columns["segundoApellido"].HeaderText = "SEGUNDO APELLIDO";
                    dgv.Columns["correo"].HeaderText = "CORREO";
                    dgv.Columns["estatus"].HeaderText = "ESTATUS";
                    dgv.Columns["tipoUsuario"].HeaderText = "TIPO DE USUARIO";

                    foreach (DataGridViewColumn col in dgv.Columns)
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgv.Columns["idUsuario"].Width = 70;
                    dgv.Columns["claveUsuario"].Width = 180;
                    dgv.Columns["Nombres"].Width = 180;
                    dgv.Columns["primerApellido"].Width = 170;
                    dgv.Columns["segundoApellido"].Width = 170;
                    dgv.Columns["correo"].Width = 210;
                    dgv.Columns["estatus"].Width = 150;
                    dgv.Columns["tipoUsuario"].Width = 120;

                    dgv.Columns["idUsuario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                };

                // ✅ Selección → llenar formulario
                dgv.SelectionChanged += (s, e) =>
                {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        DataRowView fila = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                        TxtIdUsuario.Text = fila["idUsuario"].ToString();
                        TxtClaveUsuario.Text = fila["claveUsuario"].ToString();
                        TxtNombres.Text = fila["Nombres"].ToString().Trim();
                        TxtPrimerApellido.Text = fila["primerApellido"].ToString().Trim();
                        TxtSegundoApellido.Text = fila["segundoApellido"].ToString().Trim();
                        TxtCorreo.Text = fila["correo"].ToString().Trim();
                        SeleccionarValorEnCombo(cmbEstatus, fila["estatus"].ToString());
                        SeleccionarValorEnCombo(cmbIdTipoUsuario, fila["tipoUsuario"].ToString());
                    }
                };

                if (panel1 != null)
                {
                    dgv.Location = new Point(0, panel1.Bottom);
                    dgv.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - panel1.Bottom - 40);
                }

                this.Controls.Add(dgv);
                dgv.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando usuarios: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SeleccionarValorEnCombo(ComboBox cmb, string descripcion)
        {
            foreach (DataRowView item in cmb.Items)
            {
                if (item["descripcion"].ToString().Trim() == descripcion.Trim())
                {
                    cmb.SelectedValue = item["valor"];
                    break;
                }
            }
        }

        private string EncriptarBase64(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return null;
            byte[] bytes = Encoding.Unicode.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtClaveUsuario.Text))
            {
                MessageBox.Show("Ingrese el Usuario.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbEstatus.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un Estatus.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbIdTipoUsuario.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el Tipo de Usuario.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int idUsuario = string.IsNullOrWhiteSpace(TxtIdUsuario.Text) ? 0 : Convert.ToInt32(TxtIdUsuario.Text);
                string claveUsuario = TxtClaveUsuario.Text.Trim();
                string nombres = TxtNombres.Text.Trim();
                string primerApellido = TxtPrimerApellido.Text.Trim();
                string segundoApellido = TxtSegundoApellido.Text.Trim();
                int idTipoUsuario = Convert.ToInt32(cmbIdTipoUsuario.SelectedValue);
                int idEstatus = Convert.ToInt32(cmbEstatus.SelectedValue);
                string passEncriptada = EncriptarBase64(TextPassw.Text);
                string correo = TxtCorreo.Text.Trim();
                int estatus;
                string mensaje;

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();

                    if (idUsuario == 0) // ✅ NUEVO
                    {
                        using (SqlCommand cmd = new SqlCommand("dbo.Spa_segUsuariosTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PnIdUsuario", idUsuario);
                            cmd.Parameters.AddWithValue("@PsClaveUsuario", claveUsuario);
                            cmd.Parameters.AddWithValue("@PsPassword", passEncriptada ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PnIdTipoUsuario", idTipoUsuario);
                            cmd.Parameters.AddWithValue("@PsPrimerApellido", primerApellido);
                            cmd.Parameters.AddWithValue("@PsSegundoApellido", segundoApellido);
                            cmd.Parameters.AddWithValue("@PsNombres", nombres);
                            cmd.Parameters.AddWithValue("@PsCorreo", correo);
                            cmd.Parameters.AddWithValue("@PnIdOperacion", 4);
                            cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                            cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                            cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                            SqlParameter paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                            { Direction = ParameterDirection.Output, Value = 0 };
                            cmd.Parameters.Add(paramEstatus);
                            SqlParameter paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 250)
                            { Direction = ParameterDirection.Output, Value = "" };
                            cmd.Parameters.Add(paramMensaje);

                            cmd.ExecuteNonQuery();
                            estatus = Convert.ToInt32(paramEstatus.Value ?? 0);
                            mensaje = (paramMensaje.Value ?? "").ToString().Trim();
                        }
                    }
                    else // ✅ MODIFICACIÓN
                    {
                        using (SqlCommand cmd = new SqlCommand("dbo.Spu_segUsuariosTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PnIdUsuario", idUsuario);
                            cmd.Parameters.AddWithValue("@PsPassword", passEncriptada ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PnIdTipoUsuario", idTipoUsuario);
                            cmd.Parameters.AddWithValue("@PsPrimerApellido", primerApellido);
                            cmd.Parameters.AddWithValue("@PsSegundoApellido", segundoApellido);
                            cmd.Parameters.AddWithValue("@PsNombres", nombres);
                            cmd.Parameters.AddWithValue("@PsCorreo", correo);
                            cmd.Parameters.AddWithValue("@PnIdEstatus", idEstatus);
                            cmd.Parameters.AddWithValue("@PnIdOperacion", 4);
                            cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                            cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                            cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                            SqlParameter paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                            { Direction = ParameterDirection.Output, Value = 0 };
                            cmd.Parameters.Add(paramEstatus);
                            SqlParameter paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 250)
                            { Direction = ParameterDirection.Output, Value = "" };
                            cmd.Parameters.Add(paramMensaje);

                            cmd.ExecuteNonQuery();
                            estatus = Convert.ToInt32(paramEstatus.Value ?? 0);
                            mensaje = (paramMensaje.Value ?? "").ToString().Trim();
                        }
                    }
                }

                if (estatus == 0)
                {
                    MessageBox.Show(mensaje, "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnRefrescar_Click(sender, e);
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            TxtIdUsuario.Text = "";
            TxtClaveUsuario.Text = "";
            TxtNombres.Text = "";
            TxtPrimerApellido.Text = "";
            TxtSegundoApellido.Text = "";
            TxtCorreo.Text = "";
            TextPassw.Text = "";
            cmbEstatus.SelectedIndex = -1;
            cmbIdTipoUsuario.SelectedIndex = -1;

            if (dgv != null)
            {
                this.Controls.Remove(dgv);
                dgv.Dispose();
                dgv = null;
            }
            CargarGridUsuarios();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv == null) return;

                string carpetaReportes = ConfigurationManager.AppSettings["ReportsDirectory"]
                                      ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpetaReportes))
                    Directory.CreateDirectory(carpetaReportes);

                string nombreArchivo = $"Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string rutaCompleta = Path.Combine(carpetaReportes, nombreArchivo);

                Services.ExcelExportService.ExportarUsuarios(rutaCompleta, dgv, _operacion, _claveUsuario);

                MessageBox.Show($"Exportado correctamente:\n{rutaCompleta}",
                                "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = rutaCompleta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void TxtPrimerNombre_TextChanged(object sender, EventArgs e) { }
        private void LblPrimerNombre_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
    }
}
