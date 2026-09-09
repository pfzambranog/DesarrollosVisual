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
    public partial class FrmManOperaciones : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;      // ✅ Código: CATOPE01
        private readonly string _nombreOperacion;     // ✅ Nombre: Mantenimiento Catálogo...
        private readonly string _cadenaConexion;
        private DataGridView dgv;

        // ✅ Constructor con 6 parámetros: recibe CLAVE + NOMBRE
        public FrmManOperaciones(int idUsuario, string claveUsuario, DataTable dtPermisos,
                                  string claveOperacion, string nombreOperacion, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _claveOperacion = claveOperacion;        // 
            _nombreOperacion = nombreOperacion;      // 
            _cadenaConexion = cadenaConexion;

            InitializeComponent();
        }

        private void FrmManOperaciones_Load(object sender, EventArgs e)
        {
            CargarLogo();

            //  txtOperacion.Text = $"{_claveOperacion} - {_nombreOperacion}";

            this.Text = $"{_nombreOperacion}";

            txtOperacion.Text = $"{_claveOperacion}";
            txtUsuario.Text = _claveUsuario;
            
            CargarEstatus();
            CargarGridOperaciones();

            // ✅ Habilitar Botón Procesar según autorización del usuario

            bool tienePermisoProcesar = _dtPermisos.AsEnumerable()
                 .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);


            BtnProcesar.Enabled = tienePermisoProcesar;
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
                    "WHERE  tabla = 'catOperacionesTbl' " +
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

        private void CargarGridOperaciones()
        {
            try
            {
                string sql = @"SELECT a.idOperacion,
                                      a.operacion AS ClaveOperacion,
                                      a.descripcion AS Operacion,
                                      a.llamada AS Llamada,
                                      a.ruta AS Ruta,
                                      b.descripcion AS Estatus
                               FROM   dbo.catOperacionesTbl a
                               JOIN   dbo.catGeneralesTbl b 
                                 ON b.tabla = 'catOperacionesTbl' 
                                AND b.columna = 'idEstatus'
                                AND b.valor = a.idEstatus
                               ORDER BY a.idOperacion";

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
                    dgv.Columns["idOperacion"].HeaderText = "ID OPERACIÓN";
                    dgv.Columns["ClaveOperacion"].HeaderText = "CLAVE OPERACIÓN";
                    dgv.Columns["Operacion"].HeaderText = "DESCRIPCIÓN";
                    dgv.Columns["Llamada"].HeaderText = "LLAMADA";
                    dgv.Columns["Ruta"].HeaderText = "RUTA";
                    dgv.Columns["Estatus"].HeaderText = "ESTATUS";

                    foreach (DataGridViewColumn col in dgv.Columns)
                        col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgv.Columns["idOperacion"].Width = 70;
                    dgv.Columns["ClaveOperacion"].Width = 130;
                    dgv.Columns["Operacion"].Width = 350;
                    dgv.Columns["Llamada"].Width = 200;
                    dgv.Columns["Ruta"].Width = 130;
                    dgv.Columns["Estatus"].Width = 120;
                    dgv.Columns["idOperacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                };

                dgv.SelectionChanged += (s, e) =>
                {
                    if (dgv.SelectedRows.Count > 0)
                    {
                        DataRowView fila = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                        TxtIdOperacion.Text = fila["idOperacion"].ToString();
                        TxtClaveOperacion.Text = fila["ClaveOperacion"].ToString().Trim();
                        TxtDescripcion.Text = fila["Operacion"].ToString().Trim();
                        TxtLlamada.Text = fila["Llamada"].ToString().Trim();
                        TxtRuta.Text = fila["Ruta"].ToString().Trim();
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
                MessageBox.Show($"Error cargando operaciones: {ex.Message}", "Error",
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
            if (string.IsNullOrWhiteSpace(TxtClaveOperacion.Text))
            {
                MessageBox.Show("Ingrese la Clave de Operación.", "Validación",
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
                int idOperacion = string.IsNullOrWhiteSpace(TxtIdOperacion.Text) ? 0 : Convert.ToInt32(TxtIdOperacion.Text);
                string claveOperacion = TxtClaveOperacion.Text.Trim();
                string descripcion = TxtDescripcion.Text.Trim();
                string llamada = TxtLlamada.Text.Trim();
                string ruta = TxtRuta.Text.Trim();
                int idEstatus = Convert.ToInt32(cmbEstatus.SelectedValue);
                int estatus;
                string mensaje;

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();

                    if (idOperacion == 0)
                    {
                        // ✅ ALTA — SIN @PnIdEstatus (la tabla usa DEFAULT 1)
                        using (SqlCommand cmd = new SqlCommand("dbo.Spa_catOperacionesTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PsOperacion", claveOperacion);
                            cmd.Parameters.AddWithValue("@PsDescripcion", descripcion);
                            cmd.Parameters.AddWithValue("@PsLlamada", llamada);
                            cmd.Parameters.AddWithValue("@PsRuta", ruta);
                            cmd.Parameters.AddWithValue("@PnIdOperacion", 5);
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
                        using (SqlCommand cmd = new SqlCommand("dbo.Spu_catOperacionesTbl", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PnIdOperacion", idOperacion);
                            cmd.Parameters.AddWithValue("@PsOperacion", claveOperacion);
                            cmd.Parameters.AddWithValue("@PsDescripcion", descripcion);
                            cmd.Parameters.AddWithValue("@PsLlamada", llamada);
                            cmd.Parameters.AddWithValue("@PsRuta", ruta);
                            cmd.Parameters.AddWithValue("@PnIdEstatus", idEstatus);
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


            CargarGridOperaciones();

            TxtIdOperacion.Text = "";
            TxtClaveOperacion.Text = "";
            TxtDescripcion.Text = "";
            TxtLlamada.Text = "";
            TxtRuta.Text = "";
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
                string tituloReporte = $"{_claveOperacion} - {_nombreOperacion}";
                Services.ExcelExportService.ExportarUsuarios(rutaCompleta, dgv, tituloReporte, _claveUsuario);

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
      //  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }


    }
}
