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

        private void FrmRelUsuarioOperacion_Load(object sender, EventArgs e)
        {
            this.Text = $"{_nombreOperacion}";
            txtFiltroOperaciones.Text = ""; 
            txtUsuario.Text = _claveUsuario;
            CargarLogo();

            // ✅ Al cambiar Usuario → recargar todo
            CmbUsuarios.SelectedIndexChanged += (s, args) =>
            {
                if (CmbUsuarios.SelectedValue != null && CmbUsuarios.SelectedIndex >= 0)
                    CargarGridPermisos();
                else
                    dg.DataSource = null;
            };

            // ✅ Al cambiar filtros → actualizar grid
            cmbAutorizaciones.SelectedIndexChanged += (s, args) =>
            {
                if (CmbUsuarios.SelectedValue != null) CargarGridPermisos();
            };
            cmbEstatus.SelectedIndexChanged += (s, args) =>
            {
                if (CmbUsuarios.SelectedValue != null) CargarGridPermisos();
            };
            txtFiltroOperaciones.TextChanged += (s, args) =>
            {
                if (CmbUsuarios.SelectedValue != null) CargarGridPermisos();
            };

            // ✅ Cargar listas
            CargarUsuarios();
            CargarAutorizaciones();
            CargarEstatus();

            CmbUsuarios.SelectedIndex = -1; // Sin selección al iniciar
         
            AjusteGrid();
            dg.DataSource = null; // Grid vacío al inicio

            // ✅ Permiso para botón Procesar
            bool tienePermisoProcesar = _dtPermisos.AsEnumerable()
                 .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);
            BtnProcesar.Enabled = tienePermisoProcesar;
        }


        private void CargarGridPermisos()
        {
            // ✅ Validar usuario seleccionado
            if (CmbUsuarios.SelectedValue == null || !(CmbUsuarios.SelectedValue is int idUsuarioSel))
            {
                dg.DataSource = null;
                return;
            }

            try
            {
                // ✅ Leer filtros de forma SEGURA
                string filtroOp = txtFiltroOperaciones.Text?.Trim() ?? "";

                // ✅ Leer Autorización: -1 = "(Todos)" → SIN filtro
                int? filtroAut = null;
                if (cmbAutorizaciones.SelectedValue != null)
                {
                    int valor = Convert.ToInt32(cmbAutorizaciones.SelectedValue);
                    if (valor >= 0) filtroAut = valor; // ✅ -1 = Todos → no filtra
                }

                // ✅ Leer Estatus: -1 = "(Todos)" → SIN filtro
                int? filtroEst = null;
                if (cmbEstatus.SelectedValue != null)
                {
                    int valor = Convert.ToInt32(cmbEstatus.SelectedValue);
                    if (valor >= 0) filtroEst = valor; // ✅ -1 = Todos → no filtra
                }

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    string sql = @"
SELECT DISTINCT 
    d.idUsuario, 
    a.idOperacion, 
    a.operacion AS ClaveOperacion, 
    a.descripcion AS Operacion, 
    0 AS idAutorizacion,
    0 AS idEstatus
FROM   dbo.catOperacionesTbl a 
CROSS JOIN dbo.segUsuariosTbl d
WHERE  d.idUsuario = @IdUsuario
AND    a.idEstatus = 1 
AND    NOT EXISTS  ( SELECT 1 
                     FROM   dbo.segAutOperacionesTbl b 
                     WHERE  b.idOperacion = a.idOperacion 
                     And    b.idUsuario   = d.idUsuario
                     And    b.idAutorizacion > 0)

UNION ALL

SELECT 
    d.idUsuario, 
    a.idOperacion, 
    a.operacion   AS ClaveOperacion, 
    a.descripcion AS Operacion, 
    b.idAutorizacion,
    b.idEstatus
FROM   dbo.catOperacionesTbl a 
INNER JOIN dbo.segAutOperacionesTbl b 
        ON b.idOperacion = a.idOperacion 
       AND b.idUsuario   = @IdUsuario
       AND b.idAutorizacion > 0 
       AND b.idEstatus     = 1 
INNER JOIN dbo.segUsuariosTbl d 
        ON d.idUsuario = b.idUsuario
WHERE  a.idEstatus = 1

ORDER BY Operacion, idAutorizacion";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuarioSel);
                        cn.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        // =====================================================
                        // ✅ APLICAR FILTROS CORRECTAMENTE
                        // =====================================================
                        string condicion = "";

                        // 🔍 Filtro por texto (Operación o ClaveOperacion)
                        if (!string.IsNullOrWhiteSpace(filtroOp))
                        {
                            string texto = filtroOp.Replace("'", "''");
                            condicion = $" (Operacion LIKE '%{texto}%' OR ClaveOperacion LIKE '%{texto}%') ";
                        }

                        // 🔍 Filtro por Autorización → SOLO si hay valor seleccionado
                        if (filtroAut.HasValue)
                        {
                            string parte = $" idAutorizacion = {filtroAut.Value} ";
                            condicion = condicion == "" ? parte : condicion + " AND " + parte;
                        }

                        // 🔍 Filtro por Estatus → SOLO si hay valor seleccionado
                        if (filtroEst.HasValue)
                        {
                            string parte = $" idEstatus = {filtroEst.Value} ";
                            condicion = condicion == "" ? parte : condicion + " AND " + parte;
                        }

                        // ✅ Aplicar filtro solo si hay condiciones
                        if (!string.IsNullOrWhiteSpace(condicion))
                        {
                            dt.DefaultView.RowFilter = condicion;
                        }

                        dg.DataError += (s, e) => { e.ThrowException = false; };
                        dg.DataSource = dt;
                        ConfigurarAparienciaGrid();
                        ConfigurarColumnasEditables();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\nFiltro: {txtFiltroOperaciones.Text}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void AjusteGrid()
        {   
            
             dg.EnableHeadersVisualStyles = false; 

             dg.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;   // Color de fondo
             dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;        // Color de letra
             dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

             foreach (DataGridViewColumn col in dg.Columns)
                {
                 col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }


    //  contenido de las celdas
             dg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }


        private void ConfigurarAparienciaGrid()
        {
            dg.EnableHeadersVisualStyles = false;
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // ✅ Centrar TÍTULOS de TODAS las columnas
            foreach (DataGridViewColumn col in dg.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // ✅ Alineación del contenido de las celdas
            dg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg.GridColor = Color.LightGray;
            dg.BackgroundColor = Color.LightSteelBlue;
            dg.BorderStyle = BorderStyle.Fixed3D;
            dg.RowHeadersVisible = false;
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg.AllowUserToAddRows = false;
            dg.MultiSelect = false;

        }

        private void ConfigurarColumnasEditables()
        {
            // =====================================================
            // ✅ PRIMERO: OCULTAR las columnas que NO se ven
            // =====================================================
            dg.Columns["idUsuario"].Visible = false;
            dg.Columns["idOperacion"].Visible = false;
            dg.Columns["ClaveOperacion"].Visible = false;

            // =====================================================
            // ✅ Columna Operación → SOLO LECTURA
            // =====================================================
            dg.Columns["Operacion"].HeaderText = "Operación";
            dg.Columns["Operacion"].Width = 400;
            dg.Columns["Operacion"].ReadOnly = true;
            dg.Columns["Operacion"].DefaultCellStyle.BackColor = Color.LightGray;
            dg.Columns["Operacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // =====================================================
            // ✅ Columna Autorización → REEMPLAZAR columna por COMBO
            // =====================================================
            if (dg.Columns.Contains("idAutorizacion"))
            {
                int idxAut = dg.Columns["idAutorizacion"].Index; // Posición original
                dg.Columns.RemoveAt(idxAut); 

                DataGridViewComboBoxColumn colAut = new DataGridViewComboBoxColumn
                {
                    Name = "idAutorizacion",
                    HeaderText = "Autorización",
                    Width = 210,
                    DataPropertyName = "idAutorizacion", 
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat
                };

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, Concat(valor, ' - ', descripcion) AS descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE  tabla = 'segAutOperacionesTbl' " +
                    "AND columna = 'idAutorizacion' " +
                    "ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    colAut.DataSource = dt;
                    colAut.ValueMember = "valor";       // Valor = número
                    colAut.DisplayMember = "descripcion"; // Muestra = texto
                }
                dg.Columns.Insert(idxAut, colAut); // ✅ Insertar en la MISMA posición
            }


            if (dg.Columns.Contains("idEstatus"))
            {
                int idxEst = dg.Columns["idEstatus"].Index; // Posición original
                dg.Columns.RemoveAt(idxEst); // ✅ ELIMINAR la columna simple

                DataGridViewComboBoxColumn colEst = new DataGridViewComboBoxColumn
                {
                    Name = "idEstatus",
                    HeaderText = "Estatus",
                    Width = 210,
                    DataPropertyName = "idEstatus", // ENLAZA al campo NUMÉRICO
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                    FlatStyle = FlatStyle.Flat
                };

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, Concat(valor, ' - ', descripcion) AS descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE tabla = 'segAutOperacionesTbl' " +
                    "AND columna = 'idEstatus' ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    colEst.DataSource = dt;
                    colEst.ValueMember = "valor";
                    colEst.DisplayMember = "descripcion";
                }
                dg.Columns.Insert(idxEst, colEst);

            }

            // ✅ Después de enlazar las columnas en ConfigurarColumnasEditables():

            dg.Columns["Operacion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg.Columns["Operacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg.Columns["idAutorizacion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["idAutorizacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dg.Columns["idEstatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["idEstatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void CargarAutorizaciones()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT valor, Concat(valor, ' - ', descripcion) AS descripcion 
              FROM   dbo.catGeneralesTbl 
              WHERE  tabla = 'segAutOperacionesTbl' 
                AND  columna = 'idAutorizacion' 
              ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    // ✅ Agregar opción "(Todos)" con valor = -1
                    DataRow filaTodos = dt.NewRow();
                    filaTodos["valor"] = -1;           // ✅ Valor especial NO existente
                    filaTodos["descripcion"] = "(Todos)";
                    dt.Rows.InsertAt(filaTodos, 0);

                    cmbAutorizaciones.DisplayMember = "descripcion";
                    cmbAutorizaciones.ValueMember = "valor";
                    cmbAutorizaciones.DataSource = dt;
                    cmbAutorizaciones.SelectedIndex = 0; // ✅ Por defecto = Todos
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Autorizaciones: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstatus()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, Concat(valor, ' - ', descripcion) AS descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE  tabla = 'segAutOperacionesTbl' " +
                    "AND    columna = 'idEstatus' " +
                    "ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    // ✅ Agregar opción "(Todos)" con valor = -1
                    DataRow filaTodos = dt.NewRow();
                    filaTodos["valor"] = -1;           // ✅ Valor especial NO existente
                    filaTodos["descripcion"] = "(Todos)";
                    dt.Rows.InsertAt(filaTodos, 0);

                    cmbEstatus.DisplayMember = "descripcion";
                    cmbEstatus.ValueMember = "valor";
                    cmbEstatus.DataSource = dt;
                    cmbEstatus.SelectedIndex = 0; // ✅ Por defecto = Todos
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Estatus: {ex.Message}", "Error",
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

        
        private void BtnProcesar_Click(object sender, EventArgs e)
        {
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            // ✅ Limpiar filtro de texto
            txtFiltroOperaciones.Text = "";

            // ✅ Seleccionar "(Todos)" en los combos
            if (cmbAutorizaciones.Items.Count > 0) cmbAutorizaciones.SelectedIndex = 0;
            if (cmbEstatus.Items.Count > 0) cmbEstatus.SelectedIndex = 0;

            // ✅ Quitar selección de usuario
            CmbUsuarios.SelectedIndex = -1;

            // ✅ Limpiar grid
            dg.DataSource = null;
            dg.Rows.Clear();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dg == null) return;

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

                Services.ExcelExportService.ExportarUsuarios(rutaCompleta, dg, OperReporte, tituloReporte, _claveUsuario);

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
