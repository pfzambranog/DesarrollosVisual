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
    public partial class FrmManMenus : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _codigoMenu;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;
        private DataGridView dgv;

        // ✅ Constructor con 6 parámetros: recibe CLAVE + NOMBRE
        public FrmManMenus(int idUsuario, string claveUsuario, DataTable dtPermisos,
                                  string codigoMenu, string nombreOperacion, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _codigoMenu = codigoMenu;        // 
            _nombreOperacion = nombreOperacion;      // 
            _cadenaConexion = cadenaConexion;

            InitializeComponent();
        }

        private void FrmManMenus_Load(object sender, EventArgs e)
        {
            CargarLogo();

            //  txtOperacion.Text = $"{_codigoMenu} - {_nombreOperacion}";

            this.Text = $"{_nombreOperacion}";

            txtOperacion.Text = $"{_codigoMenu}";
            txtUsuario.Text = _claveUsuario;
            
            CargarEstatus();
            CargarGridMenues();

            // ✅ Habilitar Botón Procesar según autorización del usuario

          //  bool tienePermisoProcesar = 4; //_dtPermisos.AsEnumerable()
          //       .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
         //              && Convert.ToInt32(f["idAutorizacion"]) >= 4);


         //   BtnProcesar.Enabled = tienePermisoProcesar;
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
                    "SELECT valor, descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE  tabla = 'catMenusTbl' " +
                    "AND    columna = 'idEstatus' " +
                    "ORDER BY valor", cn))
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

        private void CargarGridMenues()
        {
            try
            {
                string sql = @"SELECT a.idMenu,
                                      a.codigoMenu,
                                      a.descripcion AS menu,
                                      a.OrdenPresentacion,
                                      b.descripcion AS Estatus
                               FROM   dbo.catMenusTbl a
                               JOIN   dbo.catGeneralesTbl b 
                                 ON b.tabla = 'catMenusTbl' 
                                AND b.columna = 'idEstatus'
                                AND b.valor = a.idEstatus
                               ORDER BY a.idMenu";

                DataTable dtOperaciones = new DataTable();
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dtOperaciones);
                }

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
                dgv.DataSource = dtOperaciones;

                dgv.DataBindingComplete += (s, e) =>
                {
                    dgv.Columns["idMenu"].HeaderText = "ID Menú";
                    dgv.Columns["codigoMenu"].HeaderText = "Código Menú";
                    dgv.Columns["menu"].HeaderText = "Menú";
                    dgv.Columns["OrdenPresentacion"].HeaderText = "Orden Presentación";
                    dgv.Columns["estatus"].HeaderText = "ESTATUS";
                    foreach (DataGridViewColumn col in dgv.Columns)
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgv.Columns["idMenu"].Width = 70;
                    dgv.Columns["codigoMenu"].Width = 130;
                    dgv.Columns["menu"].Width = 350;
                    dgv.Columns["OrdenPresentacion"].Width = 200;
                    dgv.Columns["estatus"].Width = 120;
                    dgv.Columns["idMenu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                };

                dgv.SelectionChanged += (s, e) =>
                {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        DataRowView fila = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                        TxtIdMenu.Text = fila["idMenu"].ToString();
                        TxtClaveMenu.Text = fila["codigoMenu"].ToString().Trim();
                        TxtDescripcion.Text = fila["menu"].ToString().Trim();
                        TxtOrdenPresentacion.Text = fila["OrdenPresentacion"].ToString();
                        SeleccionarValorEnCombo(cmbEstatus, fila["Estatus"].ToString());
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
                MessageBox.Show($"Error cargando el detalle de los Menus: {ex.Message}", "Error",
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

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOperacion.Text))
            {
                MessageBox.Show("Ingrese la Clave de Menu.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtDescripcion.Text))
            {
                MessageBox.Show("Ingrese la Descripción.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbEstatus.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un Estatus.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int idMenu = string.IsNullOrWhiteSpace(TxtIdMenu.Text) ? 0 : Convert.ToInt32(TxtIdMenu.Text);
                string codigoMenu = TxtClaveMenu.Text.Trim();
                string descripcion = TxtDescripcion.Text.Trim();
                int OrdenPresentacion = string.IsNullOrWhiteSpace(TxtOrdenPresentacion.Text) ? 0 : Convert.ToInt32(TxtOrdenPresentacion.Text);
                int idEstatus = Convert.ToInt32(cmbEstatus.SelectedValue);
                int estatus;
                string mensaje;

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();

                    if (idMenu == 0)
                    {
                        // ✅ ALTA — SIN @PnIdEstatus (la tabla usa DEFAULT 1)
                        using (SqlCommand cmd = new SqlCommand("dbo.Spa_catMenusTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PsCodigoMenu", codigoMenu);
                            cmd.Parameters.AddWithValue("@PsDescripcion", descripcion);
                            cmd.Parameters.AddWithValue("@PnOrdenPresentacion", OrdenPresentacion);
                            cmd.Parameters.AddWithValue("@PnIdOperacion", 9);
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
                    else
                    {
                        // ✅ MODIFICACIÓN
                        using (SqlCommand cmd = new SqlCommand("dbo.Spu_catMenusTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PsCodigoMenu", codigoMenu);
                            cmd.Parameters.AddWithValue("@PsDescripcion", descripcion);
                            cmd.Parameters.AddWithValue("@PnOrdenPresentacion", OrdenPresentacion);
                            cmd.Parameters.AddWithValue("@PbIdEstatus", idEstatus);
                            cmd.Parameters.AddWithValue("@PnIdOperacion", 9);
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
                    MessageBox.Show(mensaje + "\n\nOperación guardada correctamente.",
                                    "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnRefrescar_Click(sender, e);
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {


            CargarGridMenues();
            TxtIdMenu.Text = "";
            TxtClaveMenu.Text = "";
            TxtDescripcion.Text = "";
            TxtOrdenPresentacion.Text = "";
            cmbEstatus.SelectedIndex = -1;
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

                string nombreArchivo = $"Operaciones_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string rutaCompleta = Path.Combine(carpetaReportes, nombreArchivo);

                // ✅ Pasamos TITULO como parámetro al servicio

                // string tituloReporte = $"{_codigoMenu} - {_nombreOperacion}";

                string OperReporte = $"{_codigoMenu}";
                string tituloReporte = $"{_nombreOperacion}";

                Services.ExcelExportService.ExportarUsuarios(rutaCompleta, dgv, OperReporte, tituloReporte, _claveUsuario);

                MessageBox.Show($"Exportado correctamente:\n{rutaCompleta}", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = rutaCompleta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
      //  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
    }
}
