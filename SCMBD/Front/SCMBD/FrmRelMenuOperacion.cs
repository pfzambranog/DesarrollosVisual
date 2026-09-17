using SCMBD.Properties;
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
    public partial class FrmRelMenuOperacion : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;
        private DataTable _dtOriginal;

        public FrmRelMenuOperacion(int idUsuario, string claveUsuario, DataTable dtPermisos,
                                      string claveOperacion, string nombreOperacion, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _claveOperacion = claveOperacion;
            _nombreOperacion = nombreOperacion;
            _cadenaConexion = cadenaConexion;
            InitializeComponent();
        }

        private void FrmRelMenuOperacion_Load(object sender, EventArgs e)
        {
            this.Text = $"{_nombreOperacion}";
            cmbOperaciones.SelectedIndex = -1;
            txtUsuario.Text = _claveUsuario;
            txtOperacion.Text = _claveOperacion;
            CargarLogo();
            CargarMenu();
            CargarOperacion();
            CargarEstatus();

            //

            CmbMenu.SelectedIndexChanged += (s, args) =>
            {
                if (CmbMenu.SelectedValue != null && CmbMenu.SelectedIndex >= 0)
                    CargarGridPermisos();
                else
                {
                    dg.Columns.Clear();
                    dg.Rows.Clear();
                    _dtOriginal?.Clear();
                    BtnProcesar.Enabled = false;
                    BtnBaja.Enabled = false;
                }
            };

            cmbEstatus.SelectedIndexChanged += (s, args) =>
            {
                if (CmbMenu.SelectedValue != null && CmbMenu.SelectedIndex >= 0)
                    CargarGridPermisos();
            };

            dg.SelectionChanged += (s, args) =>
            {
                BtnBaja.Enabled = dg.SelectedRows.Count > 0;
            };

            CmbMenu.SelectedIndex = -1;
            dg.Columns.Clear();
            dg.Rows.Clear();
            BtnBaja.Enabled = false;

            // 
            bool tienePermiso = _dtPermisos.AsEnumerable()
                 .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);
            BtnProcesar.Enabled = tienePermiso;
            BtnBaja.Enabled = tienePermiso && dg.SelectedRows.Count > 0;

            // 
            dg.Dock = DockStyle.Fill;
            dg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void CargarGridPermisos()
        {
            if (CmbMenu.SelectedValue == null || !(CmbMenu.SelectedValue is int idMenu))
            {
                dg.Columns.Clear();
                dg.Rows.Clear();
                _dtOriginal?.Clear();
                BtnProcesar.Enabled = false;
                BtnBaja.Enabled = false;
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    string sql = @"
                        SELECT a.idMenu, a.idOperacion, b.descripcion AS Operacion, 
                               a.secuencia AS Posicion, a.idEstatus
                        FROM   dbo.catRelMenuOperacionTbl a
                        JOIN   dbo.catOperacionesTbl      b
                        ON     b.idOperacion = a.idOperacion
                        WHERE  b.idEstatus   = 1
                        AND    a.idMenu     = @idMenu
                        ORDER BY a.secuencia, b.descripcion";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@idMenu", idMenu);
                        cn.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());

                        dg.DataError += (s, e) => { e.ThrowException = false; };
                        _dtOriginal = dt.Copy();
                        ConfigurarAparienciaGrid();
                        ConfigurarColumnasEditables(dt);
                        VerificarCambios();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dg_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            VerificarCambios();
        }

        private void VerificarCambios()
        {
            if (_dtOriginal == null || _dtOriginal.Rows.Count == 0)
            {
                BtnProcesar.Enabled = false;
                return;
            }

            bool hayCambios = false;
            foreach (DataGridViewRow filaGrid in dg.Rows)
            {
                if (filaGrid.Cells["idMenu"].Value == null || filaGrid.Cells["idOperacion"].Value == null)
                    continue;

                int idMenuFila = Convert.ToInt32(filaGrid.Cells["idMenu"].Value);
                int idOperacion = Convert.ToInt32(filaGrid.Cells["idOperacion"].Value);

                DataRow[] filasOriginal = _dtOriginal.Select(
                    $"idOperacion = {idOperacion} AND idMenu = {idMenuFila}");

                if (filasOriginal.Length > 0)
                {
                    int posOriginal = Convert.ToInt32(filasOriginal[0]["Posicion"]);
                    int estOriginal = Convert.ToInt32(filasOriginal[0]["idEstatus"]);
                    int posActual = Convert.ToInt32(filaGrid.Cells["Posicion"].Value);
                    int estActual = Convert.ToInt32(filaGrid.Cells["idEstatus"].Value);

                    if (posActual != posOriginal || estActual != estOriginal)
                    {
                        hayCambios = true;
                        break;
                    }
                }
                else
                {
                    hayCambios = true;
                    break;
                }
            }

            if (cmbOperaciones.SelectedValue != null && cmbOperaciones.SelectedIndex >= 0)
                hayCambios = true;

            BtnProcesar.Enabled = hayCambios;
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void ConfigurarAparienciaGrid()
        {
            dg.EnableHeadersVisualStyles = false;
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dg.GridColor = Color.LightGray;
            dg.BackgroundColor = Color.White;
            dg.BorderStyle = BorderStyle.FixedSingle;
            dg.RowHeadersVisible = false;
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg.AllowUserToAddRows = false;
            dg.MultiSelect = false;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // ✅ Distribuir ancho
        }

        private void ConfigurarColumnasEditables(DataTable dt)
        {
            dg.Columns.Clear();

            // Columnas ocultas
            dg.Columns.Add("idMenu", "idMenu");
            dg.Columns["idMenu"].Visible = false;

            dg.Columns.Add("idOperacion", "idOperacion");
            dg.Columns["idOperacion"].Visible = false;

            dg.Columns.Add("Operacion", "Operación");
            dg.Columns["Operacion"].FillWeight = 60; 
            dg.Columns["Operacion"].ReadOnly = true;
            dg.Columns["Operacion"].DefaultCellStyle.BackColor = Color.LightGray;
            dg.Columns["Operacion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg.Columns["Operacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            
            dg.Columns.Add("Posicion", "Posición");
            dg.Columns["Posicion"].FillWeight = 20; 
            dg.Columns["Posicion"].ReadOnly = false;
            dg.Columns["Posicion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["Posicion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            
            DataGridViewComboBoxColumn colEst = new DataGridViewComboBoxColumn
            {
                Name = "idEstatus",
                HeaderText = "Estatus",
                FillWeight = 20, 
                ReadOnly = false,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Flat
            };

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT valor, CONCAT(valor, ' - ', descripcion) AS descripcion " +
                "FROM   dbo.catGeneralesTbl " +
                "WHERE  tabla = 'segAutOperacionesTbl' " +
                "AND    columna = 'idEstatus' " +
                "ORDER BY valor", cn))
            {
                cn.Open();
                DataTable dtComboEst = new DataTable();
                dtComboEst.Load(cmd.ExecuteReader());

                DataTable dtComboSeguro = new DataTable();
                dtComboSeguro.Columns.Add("valor");
                dtComboSeguro.Columns.Add("descripcion");

                foreach (DataRow fila in dtComboEst.Rows)
                {
                    dtComboSeguro.Rows.Add(fila["valor"], fila["descripcion"]);
                }

                colEst.DataSource = dtComboSeguro;
                colEst.ValueMember = "valor";
                colEst.DisplayMember = "descripcion";
            }

            dg.Columns.Add(colEst);

            // Centrar encabezado y contenido
            dg.Columns["idEstatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["idEstatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Formato seguro
            dg.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex >= 0 && dg.Columns[e.ColumnIndex].Name == "idEstatus" && e.Value != null)
                {
                    if (e.Value is bool bVal)
                        e.Value = bVal ? "1" : "0";
                    else
                        e.Value = e.Value.ToString();
                    e.FormattingApplied = true;
                }
            };

            // Cargar filas
            dg.Rows.Clear();
            foreach (DataRow fila in dt.Rows)
            {
                int idx = dg.Rows.Add();
                DataGridViewRow filaG = dg.Rows[idx];
                filaG.Cells["idMenu"].Value = fila["idMenu"];
                filaG.Cells["idOperacion"].Value = fila["idOperacion"];
                filaG.Cells["Operacion"].Value = fila["Operacion"];
                filaG.Cells["Posicion"].Value = fila["Posicion"];
                filaG.Cells["idEstatus"].Value = fila["idEstatus"];
            }

            dg.CellEndEdit += dg_CellEndEdit;
        }

        private void CargarMenu()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT idMenu, descripcion, OrdenPresentacion " +
                    "FROM   dbo.catMenusTbl " +
                    "WHERE  idEstatus = 1 " +
                    "ORDER BY OrdenPresentacion", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    CmbMenu.DisplayMember = "descripcion";
                    CmbMenu.ValueMember = "idMenu";
                    CmbMenu.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Menús: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarOperacion()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT idOperacion, descripcion AS Operacion " +
                    "FROM   dbo.catOperacionesTbl a " +
                    "WHERE  idEstatus = 1 " +
                    "AND    Not Exists (SELECT Top 1 1 " +
                    "                   FROM   dbo.catRelMenuOperacionTbl" +
                    "                   Where  idOperacion = a.idOperacion) " +
                    "ORDER BY descripcion", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbOperaciones.DisplayMember = "Operacion";
                    cmbOperaciones.ValueMember = "idOperacion";
                    cmbOperaciones.DataSource = dt;
                    cmbOperaciones.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Operaciones: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstatus()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT valor, CONCAT(valor, ' - ', descripcion) AS descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE  tabla = 'segAutOperacionesTbl' " +
                    "AND    columna = 'idEstatus' " +
                    "ORDER BY valor", cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbEstatus.DisplayMember = "descripcion";
                    cmbEstatus.ValueMember = "valor";
                    cmbEstatus.DataSource = dt;
                    cmbEstatus.Text = "";
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
            try
            {
                if (CmbMenu.SelectedValue == null || !(CmbMenu.SelectedValue is int idMenuSel))
                {
                    MessageBox.Show("Seleccione un Menú.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                StringBuilder json = new StringBuilder();
                json.Append("[");
                bool primero = true;

                foreach (DataGridViewRow filaGrid in dg.Rows)
                {
                    if (filaGrid.Cells["idMenu"].Value == null || filaGrid.Cells["idOperacion"].Value == null)
                        continue;

                    int idMenuFila = Convert.ToInt32(filaGrid.Cells["idMenu"].Value);
                    int idOperacionFila = Convert.ToInt32(filaGrid.Cells["idOperacion"].Value);
                    int posActual = Convert.ToInt32(filaGrid.Cells["Posicion"].Value);
                    int estActual = Convert.ToInt32(filaGrid.Cells["idEstatus"].Value);

                    bool esModificada = true;
                    if (_dtOriginal != null && _dtOriginal.Rows.Count > 0)
                    {
                        DataRow[] filasOriginal = _dtOriginal.Select(
                            $"idOperacion = {idOperacionFila} AND idMenu = {idMenuFila}");
                        if (filasOriginal.Length > 0)
                        {
                            int posOriginal = Convert.ToInt32(filasOriginal[0]["Posicion"]);
                            int estOriginal = Convert.ToInt32(filasOriginal[0]["idEstatus"]);
                            esModificada = (posActual != posOriginal || estActual != estOriginal);
                        }
                    }

                    if (esModificada)
                    {
                        if (!primero) json.Append(",");
                        json.Append($"{{\"idMenu\":{idMenuFila}," +
                                          $"\"idOperacion\":{idOperacionFila}," +
                                          $"\"secuencia\":{posActual}," +
                                          $"\"idEstatus\":{estActual}}}");
                        primero = false;
                    }
                }

                if (cmbOperaciones.SelectedValue != null && cmbOperaciones.SelectedValue is int idOpNueva && idOpNueva > 0)
                {
                    if (!primero) json.Append(",");
                    json.Append($"{{\"idMenu\":{idMenuSel}," +
                                      $"\"idOperacion\":{idOpNueva}," +
                                      $"\"secuencia\":0," +
                                      $"\"idEstatus\":1}}");
                    primero = false;
                }

                json.Append("]");
                string jsonDatos = json.ToString();

                if (jsonDatos == "[]")
                {
                    MessageBox.Show("No hay cambios para procesar.", "Información",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("Spp_catRelMenuOperacionTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsJasonIn", jsonDatos);
                    cmd.Parameters.AddWithValue("@PsOperacion", txtOperacion.Text);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                    cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                    SqlParameter paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(paramEstatus);

                    SqlParameter paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output,
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = paramEstatus.Value == DBNull.Value ? 9999 : Convert.ToInt32(paramEstatus.Value);
                    string mensaje = paramMensaje.Value?.ToString()?.Trim() ?? "";

                    if (estatus == 0)
                    {
                        MessageBox.Show("Actualización realizada correctamente.",
                                        "Procesamiento Exitoso",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbOperaciones.SelectedIndex = -1;
                        CargarGridPermisos();
                    }
                    else if (estatus == 1)
                    {
                        var dtErrores = ParsearErroresJson(mensaje);
                        if (dtErrores != null && dtErrores.Rows.Count > 0)
                        {
                            string rutaLogo = Path.Combine(Application.StartupPath,
                                ConfigurationManager.AppSettings["Imagenes"] ?? "", "Logo.png");
                            using (FrmListaErrores frmErr = new FrmListaErrores(dtErrores, rutaLogo))
                            {
                                frmErr.ShowDialog(this);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"⚠️ {mensaje}", "Errores de Validación",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"⚠️ {mensaje}", "Procesamiento",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Baja
        private void BtnBaja_Click(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione el registro a eliminar.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow filaSel = dg.SelectedRows[0];
            int idMenu = Convert.ToInt32(filaSel.Cells["idMenu"].Value);
            int idOperacion = Convert.ToInt32(filaSel.Cells["idOperacion"].Value);
            string nombreOp = filaSel.Cells["Operacion"].Value?.ToString() ?? "";

            if (MessageBox.Show($"¿Eliminar la operación?\n\n{nombreOp}",
                                "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("Spd_catRelMenuOperacionTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PnIdMenu", idMenu);
                    cmd.Parameters.AddWithValue("@PnIdOperacion", idOperacion);
                    cmd.Parameters.AddWithValue("@PsOperacion", txtOperacion.Text);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                    cmd.Parameters.AddWithValue("@PsIpAct",         DBNull.Value);
                    cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                    SqlParameter paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(paramEstatus);

                    SqlParameter paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.NVarChar, 250)
                    {
                        Direction = ParameterDirection.Output,
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = paramEstatus.Value == DBNull.Value ? 9999 : Convert.ToInt32(paramEstatus.Value);
                    string mensaje = paramMensaje.Value?.ToString()?.Trim() ?? "";

                    if (estatus == 0)
                    {
                        MessageBox.Show("✅ Registro eliminado correctamente.", "Baja",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGridPermisos();
                    }
                    else
                    {
                        MessageBox.Show($"⚠️ {mensaje}", "Error en Baja",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            cmbOperaciones.SelectedIndex = -1;
            CmbMenu.SelectedIndex = -1;
            cmbEstatus.Text = "";
            dg.Columns.Clear();
            dg.Rows.Clear();
            _dtOriginal?.Clear();
            BtnProcesar.Enabled = false;
            BtnBaja.Enabled = false;
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dg == null) return;
                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"] ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                string archivo = $"RelMenuOperacion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);
                Services.ExcelExportService.ExportarUsuarios(ruta, dg, _claveOperacion, _nombreOperacion, _claveUsuario);
                MessageBox.Show($"✅ Exportado:\n{ruta}", "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = ruta, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ParsearErroresJson(string jsonTexto)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Secuencia", typeof(int));
            dt.Columns.Add("Menu", typeof(string));
            dt.Columns.Add("Operación", typeof(string));
            dt.Columns.Add("Posicion", typeof(int));
            dt.Columns.Add("Estatus", typeof(int));
            dt.Columns.Add("N° Error", typeof(int));
            dt.Columns.Add("Mensaje", typeof(string));

            if (string.IsNullOrWhiteSpace(jsonTexto)) return dt;

            try
            {
                string contenido = jsonTexto.Trim().Trim('[', ']');
                if (string.IsNullOrWhiteSpace(contenido)) return dt;

                string[] objetos = contenido.Split(new[] { "}," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string obj in objetos)
                {
                    string fila = obj.Trim().Trim('{', '}');
                    dt.Rows.Add(
                        LeerValorInt(fila, "secuencia") ?? 0,
                        LeerValorCadena(fila, "menu"),
                        LeerValorCadena(fila, "operacion"),
                        LeerValorInt(fila, "secuencia") ?? 0,
                        LeerValorInt(fila, "idEstatus") ?? 0,
                        LeerValorInt(fila, "error") ?? 0,
                        LeerValorCadena(fila, "mensaje")
                    );
                }
            }
            catch { }
            return dt;
        }

        private int? LeerValorInt(string texto, string campo)
        {
            string p = $"\"{campo}\":";
            int i = texto.IndexOf(p);
            if (i < 0) return null;
            string v = texto.Substring(i + p.Length).Trim();
            if (v.StartsWith("null", StringComparison.OrdinalIgnoreCase)) return null;
            int f = v.IndexOf(','); if (f < 0) f = v.IndexOf('}'); if (f < 0) f = v.Length;
            return int.TryParse(v.Substring(0, f), out int n) ? n : (int?)null;
        }

        private string LeerValorCadena(string texto, string campo)
        {
            string p = $"\"{campo}\":\"";
            int i = texto.IndexOf(p);
            if (i < 0) return "";
            int f = texto.IndexOf('"', i + p.Length);
            return f < 0 ? "" : texto.Substring(i + p.Length, f - i - p.Length).Replace("\\\"", "\"");
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
    }
}
