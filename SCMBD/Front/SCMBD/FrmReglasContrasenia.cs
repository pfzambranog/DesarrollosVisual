using SCMBD.Properties;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmReglasContrasenia : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;

        public FrmReglasContrasenia(int idUsuario, string claveUsuario, DataTable dtPermisos,
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

        private void FrmReglasContrasenia_Load(object sender, EventArgs e)
        {
            this.Text = _nombreOperacion;
            txtUsuario.Text = _claveUsuario;
            txtOperacion.Text = _claveOperacion;
            CargarLogo();
            CargarCombos();
            CargarGridMotivoCorreo();
            LimpiarCampos();

            bool puedeEditar = _dtPermisos.AsEnumerable()
                .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 2);

            BtnProcesar.Enabled = puedeEditar;
            BtnBaja.Enabled = false;
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void CargarCombos()
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT valor, CASE WHEN valor=1 THEN 'SI' ELSE 'NO' END AS descripcion " +
                "FROM dbo.catGeneralesTbl " +
                "WHERE tabla = 'segReglasContrasenaTbl' " +
                "AND columna = 'esRequerido' " +
                "ORDER BY valor", cn))
            {
                cn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                CmbRequerido.DisplayMember = "descripcion";
                CmbRequerido.ValueMember = "valor";
                CmbRequerido.DataSource = dt;
            }

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(
                "Select -1 valor, ' ' descripcion " +
                "Union " +
                "SELECT valor, descripcion FROM dbo.catGeneralesTbl " +
                "WHERE tabla = 'segReglasContrasenaTbl' " +
                "AND columna = 'idEstatus' " +
                "ORDER BY valor", cn))
            {
                cn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                cmbEstatus.DisplayMember = "descripcion";
                cmbEstatus.ValueMember = "valor";
                cmbEstatus.DataSource = dt;
            }
        }

        private void CargarGridMotivoCorreo()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    string sql = @"
SELECT 
    idRegla,
    codRegla,
    nombreRegla,
    descripcion,
    esRequerido,
    CASE WHEN esRequerido = 1 THEN 'SI' ELSE 'NO' END AS RequeridoTexto,
    valorMinimo,
    idEstatus,
    CASE 
        WHEN idEstatus = 0 THEN 'Regla Deshabilitada'
        WHEN idEstatus = 1 THEN 'Regla Habilitada'
        ELSE 'Desconocido'
    END AS EstatusTexto
FROM dbo.segReglasContrasenaTbl
ORDER BY codRegla";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cn.Open();
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        ConfigurarAparienciaGrid(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando datos: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dg_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dg == null || dg.CurrentRow == null || dg.CurrentRow.Index < 0)
                return;

            SeleccionarFila(dg.CurrentRow.Index);
        }

        private void Dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SeleccionarFila(e.RowIndex);
                BtnBaja.Enabled = true;
            }
            else
            {
                BtnBaja.Enabled = false;
            }
        }

        private void Dg_SelectionChanged(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count > 0)
                dg.SelectedRows[0].Selected = true;
        }

        private void SeleccionarFila(int indice)
        {
            DataGridViewRow fila = dg.Rows[indice];

            TxtRegla.Text = fila.Cells["codRegla"].Value?.ToString().Trim() ?? "";
            TxtNombreRegla.Text = fila.Cells["nombreRegla"].Value?.ToString().Trim() ?? "";
            TxtDescripcion.Text = fila.Cells["descripcion"].Value?.ToString().Trim() ?? "";

            if (fila.Cells["esRequerido"].Value != null && fila.Cells["esRequerido"].Value != DBNull.Value)
                CmbRequerido.SelectedValue = Convert.ToInt32(fila.Cells["esRequerido"].Value);

            TxtValorMinimo.Text = fila.Cells["valorMinimo"].Value?.ToString().Trim() ?? "";

            if (fila.Cells["idEstatus"].Value != null && fila.Cells["idEstatus"].Value != DBNull.Value)
                cmbEstatus.SelectedValue = Convert.ToInt32(fila.Cells["idEstatus"].Value);

            BtnBaja.Enabled = true;
        }

        private void ConfigurarAparienciaGrid(DataTable dt)
        {
            dg.Columns.Clear();
            dg.EnableHeadersVisualStyles = false;
            dg.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.SteelBlue;
            dg.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dg.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dg.RowHeadersVisible = false;
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg.AllowUserToAddRows = false;
            dg.MultiSelect = false;

            dg.Columns.Add("codRegla", "Regla");
            dg.Columns["codRegla"].Width = 90;
            dg.Columns["codRegla"].ReadOnly = true;

            dg.Columns.Add("nombreRegla", "Nombre");
            dg.Columns["nombreRegla"].Width = 250;
            dg.Columns["nombreRegla"].ReadOnly = true;
            dg.Columns["nombreRegla"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg.Columns.Add("descripcion", "Descripción");
            dg.Columns["descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dg.Columns["descripcion"].ReadOnly = true;
            dg.Columns["descripcion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg.Columns.Add("RequeridoTexto", "Requerido");
            dg.Columns["RequeridoTexto"].Width = 100;
            dg.Columns["RequeridoTexto"].ReadOnly = true;

            dg.Columns.Add("valorMinimo", "Valor Mínimo");
            dg.Columns["valorMinimo"].Width = 110;
            dg.Columns["valorMinimo"].ReadOnly = true;

            dg.Columns.Add("EstatusTexto", "Estatus");
            dg.Columns["EstatusTexto"].Width = 150;
            dg.Columns["EstatusTexto"].ReadOnly = true;
            dg.Columns["EstatusTexto"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;


            dg.Columns.Add("idRegla", "idRegla"); dg.Columns["idRegla"].Visible = false;
            dg.Columns.Add("esRequerido", "esRequerido"); dg.Columns["esRequerido"].Visible = false;
            dg.Columns.Add("idEstatus", "idEstatus"); dg.Columns["idEstatus"].Visible = false;

            foreach (DataRow fila in dt.Rows)
            {
                int idx = dg.Rows.Add();
                DataGridViewRow f = dg.Rows[idx];
                f.Cells["codRegla"].Value = fila["codRegla"];
                f.Cells["nombreRegla"].Value = fila["nombreRegla"];
                f.Cells["descripcion"].Value = fila["descripcion"];
                f.Cells["RequeridoTexto"].Value = fila["RequeridoTexto"];
                f.Cells["valorMinimo"].Value = fila["valorMinimo"];
                f.Cells["EstatusTexto"].Value = fila["EstatusTexto"];
                f.Cells["idRegla"].Value = fila["idRegla"];
                f.Cells["esRequerido"].Value = fila["esRequerido"];
                f.Cells["idEstatus"].Value = fila["idEstatus"];
            }

            dg.CellClick -= Dg_CellClick;
            dg.CurrentCellChanged -= Dg_CurrentCellChanged;
            dg.SelectionChanged -= Dg_SelectionChanged;
            dg.CellClick += Dg_CellClick;
            dg.CurrentCellChanged += Dg_CurrentCellChanged;
            dg.SelectionChanged += Dg_SelectionChanged;

            if (dg.Rows.Count > 0)
            {
                dg.ClearSelection();
                dg.CurrentCell = null;
                dg.Rows[0].Selected = true;
                dg.CurrentCell = dg.Rows[0].Cells[0];
            }
        }

        private void LimpiarCampos()
        {
            TxtRegla.Text = "";
            TxtNombreRegla.Text = "";
            TxtDescripcion.Text = "";
            CmbRequerido.SelectedIndex = -1;
            TxtValorMinimo.Text = "";
            cmbEstatus.SelectedIndex = -1;
            BtnBaja.Enabled = false;
        }

        private int ObtenerIdOperacionActual()
        {
            return _dtPermisos.AsEnumerable()
                .First(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim())
                .Field<int>("idOperacion");
        }

        private string EscaparJson(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            return texto
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r");
        }

        private DataTable ParsearErroresJson(string jsonTexto)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Secuencia", typeof(int));
            dt.Columns.Add("Regla", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Requerido", typeof(string));
            dt.Columns.Add("Valor Mínimo", typeof(int));
            dt.Columns.Add("Estatus", typeof(int));
            dt.Columns.Add("N° Error", typeof(int));
            dt.Columns.Add("Mensaje", typeof(string));

            if (string.IsNullOrWhiteSpace(jsonTexto))
                return dt;

            try
            {
                string contenido = jsonTexto.Trim().Trim('[', ']');
                if (string.IsNullOrWhiteSpace(contenido))
                    return dt;

                string[] objetos = contenido.Split(new[] { "}," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string obj in objetos)
                {
                    string fila = obj.Trim().Trim('{', '}');
                    dt.Rows.Add(
                        LeerValorInt(fila, "sec") ?? 0,
                        LeerValorCadena(fila, "codRegla"),
                        LeerValorCadena(fila, "nombreRegla"),
                        LeerValorCadena(fila, "descripcion"),
                        LeerValorBool(fila, "esRequerido"),
                        LeerValorInt(fila, "valorMinimo") ?? 0,
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

        private string LeerValorBool(string texto, string campo)
        {
            string p = $"\"{campo}\":";
            int i = texto.IndexOf(p);
            if (i < 0) return "";
            string v = texto.Substring(i + p.Length).Trim();
            if (v.StartsWith("true", StringComparison.OrdinalIgnoreCase)) return "SI";
            if (v.StartsWith("false", StringComparison.OrdinalIgnoreCase)) return "NO";
            return v.StartsWith("1") ? "SI" : "NO";
        }

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtRegla.Text))
                {
                    MessageBox.Show("Ingrese la clave de la regla.", "Validación",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtRegla.Focus();
                    return;
                }

                string codRegla = TxtRegla.Text.Trim();
                string nombreRegla = TxtNombreRegla.Text?.Trim() ?? "";
                string descripcion = TxtDescripcion.Text?.Trim() ?? "";

                int esRequerido = CmbRequerido.SelectedValue != null
                    ? Convert.ToInt32(CmbRequerido.SelectedValue)
                    : 0;

                int valorMinimo = int.TryParse(TxtValorMinimo.Text, out int vm) ? vm : 0;

                int idEstatus = cmbEstatus.SelectedValue != null
                    ? Convert.ToInt32(cmbEstatus.SelectedValue)
                    : 0;

                string jsonDatos = $@"
[{{
    ""codRegla"": ""{EscaparJson(codRegla)}"",
    ""nombreRegla"": ""{EscaparJson(nombreRegla)}"",
    ""descripcion"": ""{EscaparJson(descripcion)}"",
    ""esRequerido"": {esRequerido},
    ""valorMinimo"": {valorMinimo},
    ""idEstatus"": {idEstatus}
}}]";

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("Spp_segReglasContrasenaTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsJasonIn", jsonDatos);
                    cmd.Parameters.AddWithValue("@PsOperacion", _claveOperacion);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                    cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                    SqlParameter pEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(pEstatus);

                    SqlParameter pMensaje = new SqlParameter("@PsMensaje", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = Convert.ToInt32(pEstatus.Value);
                    string mensaje = pMensaje.Value?.ToString()?.Trim() ?? "";

                    if (estatus == 0)
                    {
                        MessageBox.Show("✅ Regla guardada correctamente.", "Éxito",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGridMotivoCorreo();
                        LimpiarCampos();
                    }
                    else if (estatus == 1 && !string.IsNullOrWhiteSpace(mensaje))
                    {
                        DataTable dtErrores = ParsearErroresJson(mensaje);
                        if (dtErrores.Rows.Count > 0)
                        {
                            FrmListaErrores frm = new FrmListaErrores(dtErrores);
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"⚠️ {mensaje}", "Aviso",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBaja_Click(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione la regla a eliminar.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow fila = dg.SelectedRows[0];
            string codRegla = fila.Cells["codRegla"].Value?.ToString()?.Trim() ?? "";
            string nombreRegla = fila.Cells["nombreRegla"].Value?.ToString()?.Trim() ?? "";

            if (MessageBox.Show($"¿Eliminar la regla?\n\n{codRegla} — {nombreRegla}",
                                "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("Spd_segReglasContrasenaTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ✅ SOLO los 5 parámetros que el SP espera — SIN @PsIpAct ni @PsMacAddressAct
                    cmd.Parameters.AddWithValue("@PsCodRegla", codRegla);
                    cmd.Parameters.AddWithValue("@PnIdOperacionAct", ObtenerIdOperacionActual());
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);

                    SqlParameter pEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(pEstatus);

                    SqlParameter pMensaje = new SqlParameter("@PsMensaje", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = Convert.ToInt32(pEstatus.Value);
                    string mensaje = pMensaje.Value?.ToString()?.Trim() ?? "";

                    if (estatus == 0)
                    {
                        MessageBox.Show("✅ Regla eliminada.", "Éxito",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarGridMotivoCorreo();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show($"⚠️ {mensaje}", "Aviso",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            CargarGridMotivoCorreo();
            LimpiarCampos();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dg == null || dg.Rows.Count == 0) return;
                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"] ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                string archivo = $"ReglasContrasenia_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);
                string operReporte = _claveOperacion;
                string tituloReporte = _nombreOperacion;

                Services.ExcelExportService.ExportarUsuarios(ruta, dg, operReporte, tituloReporte, _claveUsuario);

                MessageBox.Show($"✅ Exportado:\n{ruta}", "Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exportando: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
        private void Panel1_Paint(object sender, PaintEventArgs e) { }
    }
}
