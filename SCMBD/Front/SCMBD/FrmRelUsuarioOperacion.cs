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
    public partial class FrmRelUsuarioOperacion : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;      // ✅ Código: CATOPE01
        private readonly string _nombreOperacion;     // ✅ Nombre: Mantenimiento Catálogo...
        private readonly string _cadenaConexion;
        private DataGridView dgv;

        // ✅ Constructor con 6 parámetros: recibe CLAVE + NOMBRE
        public FrmRelUsuarioOperacion(int idUsuario, string claveUsuario, DataTable dtPermisos,
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

            this.Text = $"{_nombreOperacion}";

            txtOperacion.Text = $"{_claveOperacion}";
            txtUsuario.Text = _claveUsuario;
            

            CargarLogo();
            CargarUsuarios();
            CargarAutorizaciones();
            CargarEstatus();

            CmbUsuarios.Text = "";
            cmbAutorizaciones.Text = "";
            cmbEstatus.Text = "";

            // 

            bool tienePermisoProcesar = _dtPermisos.AsEnumerable()
                 .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);


            BtnProcesar.Enabled = tienePermisoProcesar;
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void CargarAutorizaciones()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT valor, descripcion 
                      FROM   dbo.catGeneralesTbl 
                      WHERE  tabla = 'segAutOperacionesTbl' 
                        AND  columna = 'idAutorizacion' 
                      ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbAutorizaciones.DisplayMember = "descripcion";
                    cmbAutorizaciones.ValueMember = "valor";
                    cmbAutorizaciones.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Autorizaciones: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT idUsuario, 
                             CONCAT(claveUsuario, ' - ', nombres, ' ', primerApellido, ' ', segundoApellido) AS Usuario
                      FROM   dbo.segUsuariosTbl 
                      WHERE  idEstatus = 1 
                      ORDER BY idUsuario", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    CmbUsuarios.DisplayMember = "Usuario";
                    CmbUsuarios.ValueMember = "idUsuario";
                    CmbUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Usuarios: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {


            CmbUsuarios.Text = "";
            cmbAutorizaciones.Text = "";
            cmbEstatus.Text = "";
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

                // string tituloReporte = $"{_claveOperacion} - {_nombreOperacion}";

                string OperReporte = $"{_claveOperacion}";
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
