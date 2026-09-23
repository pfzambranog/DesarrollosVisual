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
    public partial class FrmManMotivoCorreo : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;
        private DataTable _dtOriginal;
        private DataGridView _dgv;

        // Valores originales del registro seleccionado
        private int _valorOriginalCompartir;
        private int _valorOriginalEstatus;
        private bool _hayRegistroSeleccionado;

        public FrmManMotivoCorreo(int idUsuario, string claveUsuario, DataTable dtPermisos,
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

        private void FrmManMotivoCorreo_Load(object sender, EventArgs e)
        {
            CargarLogo();
            Text = _nombreOperacion;
            txtOperacion.Text = _claveOperacion;
            txtUsuario.Text = _claveUsuario;

            ConfigurarGrid();
            CargarEstatus();
            CargarPermitirCompartir();
            CargarDatos();

            // ✅ Corregido: Comparación robusta de permisos
            bool tienePermisoProcesar = _dtPermisos.AsEnumerable()
                .Any(f => f["claveOperacion"].ToString().Trim().Equals(_claveOperacion.Trim(), StringComparison.OrdinalIgnoreCase)
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);
            bool tienePermisoBaja = _dtPermisos.AsEnumerable()
                .Any(f => f["claveOperacion"].ToString().Trim().Equals(_claveOperacion.Trim(), StringComparison.OrdinalIgnoreCase)
                       && Convert.ToInt32(f["idAutorizacion"]) >= 8);

            BtnProcesar.Enabled = tienePermisoProcesar;
            BtnBaja.Enabled = false; // Solo se activa al seleccionar registro
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void ConfigurarGrid()
        {
            _dgv = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            _dgv.CellClick += Dgv_CellClick;
            _dgv.CurrentCellChanged += Dgv_CurrentCellChanged;
            _dgv.SelectionChanged += Dgv_SelectionChanged; // ✅ Nuevo evento
            Controls.Add(_dgv);
            _dgv.BringToFront();
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            // ✅ Activar/desactivar botón Baja según selección
            BtnBaja.Enabled = _dgv.SelectedRows.Count > 0;
        }

        private void AplicarEncabezados()
        {
            foreach (DataGridViewColumn col in _dgv.Columns)
            {
                string nombre = col.Name.ToLower();
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                switch (nombre)
                {
                    case "idmotivo":
                        col.HeaderText = "ID";
                        col.FillWeight = 8;
                        break;
                    case "descripcion":
                        col.HeaderText = "Descripción";
                        col.FillWeight = 25;
                        break;
                    case "titulo":
                        col.HeaderText = "Título";
                        col.FillWeight = 25;
                        break;
                    case "perfilcorreo":
                        col.HeaderText = "Perfil";
                        col.FillWeight = 15;
                        break;
                    case "permitecompartir":
                        col.HeaderText = "Compartir";
                        col.FillWeight = 10;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "estatus":
                        col.HeaderText = "Estatus";
                        col.FillWeight = 12;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "cuerpo":
                    case "html":
                    case "url":
                    case "idestatus":
                        col.Visible = false;
                        break;
                }
            }
        }

        private void CargarPermitirCompartir()
        {
            cmbCompartir.DisplayMember = "Descripcion";
            cmbCompartir.ValueMember = "Valor";
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT -1 AS Valor, ' ' AS Descripcion " +
                    "UNION ALL SELECT 0 AS Valor, 'No' AS Descripcion " +
                    "UNION ALL SELECT 1 AS Valor, 'Sí' AS Descripcion ORDER BY Valor", cn))
                {
                    cn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbCompartir.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando valores de Compartir: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstatus()
        {
            cmbEstatus.DisplayMember = "Descripcion";
            cmbEstatus.ValueMember = "Valor";
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT -1 AS Valor, ' ' AS Descripcion " +
                    "UNION ALL SELECT valor, descripcion " +
                    "FROM   dbo.catGeneralesTbl " +
                    "WHERE  tabla = 'conMotivosCorreoTbl' " +
                    "AND    columna = 'idEstatus' " +
                    "ORDER BY Valor", cn))
                {
                    cn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    cmbEstatus.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando Estatus: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatos()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("dbo.Spc_conMotivosCorreoTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsOperacion", _claveOperacion);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);

                    var paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    { Direction = ParameterDirection.Output, Value = 0 };
                    cmd.Parameters.Add(paramEstatus);
                    var paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 250)
                    { Direction = ParameterDirection.Output, Value = "" };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    _dtOriginal = dt.Copy();
                    _dgv.DataSource = dt;
                    AplicarEncabezados();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando datos: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_CurrentCellChanged(object sender, EventArgs e)
        {
            if (_dgv.CurrentRow == null) return;
            LlenarCamposDesdeFila(_dgv.CurrentRow);
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                LlenarCamposDesdeFila(_dgv.Rows[e.RowIndex]);
        }

        private void LlenarCamposDesdeFila(DataGridViewRow fila)
        {
            _hayRegistroSeleccionado = true;

            // ✅ Guardar valores originales para usar si el combo queda en -1
            _valorOriginalCompartir = LeerValorEntero(fila, "permiteCompartir");
            _valorOriginalEstatus = LeerValorEntero(fila, "idEstatus");

            TxtIdMotivo.Text = LeerValor(fila, "idMotivo");
            TxtDescripcion.Text = LeerValor(fila, "descripcion");
            TxtTitulo.Text = LeerValor(fila, "titulo");
            TxtCuerpo.Text = LeerValor(fila, "cuerpo");
            TxtHTML.Text = LeerValor(fila, "html");
            TxtURL.Text = LeerValor(fila, "url");
            TxtPerfilCorreo.Text = LeerValor(fila, "perfilCorreo");

            SeleccionarEnCombo(cmbCompartir, _valorOriginalCompartir.ToString());
            SeleccionarEnCombo(cmbEstatus, _valorOriginalEstatus.ToString());
        }

        private string LeerValor(DataGridViewRow fila, string nombreColumna)
        {
            return fila.Cells[nombreColumna]?.Value?.ToString()?.Trim() ?? "";
        }

        private int LeerValorEntero(DataGridViewRow fila, string nombreColumna)
        {
            string valor = LeerValor(fila, nombreColumna);
            return int.TryParse(valor, out int n) ? n : 0;
        }

        private void SeleccionarEnCombo(ComboBox cmb, string valor)
        {
            if (cmb.DataSource is DataTable dt)
            {
                foreach (DataRowView row in dt.DefaultView)
                {
                    if (row["Valor"].ToString() == valor)
                    {
                        cmb.SelectedValue = row["Valor"];
                        break;
                    }
                }
            }
        }

        private int ObtenerValorComboConHerencia(ComboBox cmb, int valorOriginal)
        {
            if (cmb.SelectedValue == null) return valorOriginal;
            int valor = Convert.ToInt32(cmb.SelectedValue);
            // ✅ Si está en "-1" → usar el valor original del registro
            return valor == -1 ? valorOriginal : valor;
        }

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDescripcion.Text))
            {
                MessageBox.Show("Ingrese la Descripción.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cuerpo = TxtCuerpo.Text?.Trim() ?? "";
            string html = TxtHTML.Text?.Trim() ?? "";

            // ✅ Si HTML está vacío → convertir desde Cuerpo
            if (string.IsNullOrWhiteSpace(html))
            {
                html = ConvertirTextoPlanoAHtml(cuerpo);
                TxtHTML.Text = html;
            }

            // ✅ Validar estructura HTML
            if (!ValidarEstructuraHtml(html, out string mensajeError))
            {
                MessageBox.Show(
                    $"El campo HTML tiene estructura incorrecta:\n\n{mensajeError}",
                    "Validación de HTML",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // ✅ Confirmar si no hay etiquetas HTML
            if (!TieneEtiquetasHtml(html))
            {
                var resp = MessageBox.Show(
                    "El contenido no presenta formato HTML.\n¿Desea continuar de todos modos?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resp == DialogResult.No)
                    return;
            }

            try
            {
                int idMotivo = string.IsNullOrWhiteSpace(TxtIdMotivo.Text) ? 0 : Convert.ToInt32(TxtIdMotivo.Text);

                // ✅ Respetar valor original si combo está en "-1"
                int permiteCompartir = ObtenerValorComboConHerencia(cmbCompartir, _valorOriginalCompartir);
                int idEstatus = ObtenerValorComboConHerencia(cmbEstatus, _valorOriginalEstatus);

                // ✅ Construir JSON con valores correctos
                var sb = new StringBuilder();
                sb.Append("[{");
                sb.Append($"\"idMotivo\":{idMotivo},");
                sb.Append($"\"descripcion\":\"{EscaparJson(TxtDescripcion.Text.Trim())}\",");
                sb.Append($"\"titulo\":\"{EscaparJson(TxtTitulo.Text.Trim())}\",");
                sb.Append($"\"cuerpo\":\"{EscaparJson(cuerpo)}\",");
                sb.Append($"\"html\":\"{EscaparJson(html)}\",");
                sb.Append($"\"url\":\"{EscaparJson(TxtURL.Text.Trim())}\",");
                sb.Append($"\"perfilCorreo\":\"{EscaparJson(TxtPerfilCorreo.Text.Trim())}\",");
                sb.Append($"\"permiteCompartir\":{permiteCompartir},");
                sb.Append($"\"idEstatus\":{idEstatus}");
                sb.Append("}]");
                string json = sb.ToString();

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("dbo.Spp_conMotivosCorreoTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsJasonIn", json);
                    cmd.Parameters.AddWithValue("@PsOperacion", _claveOperacion);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                    cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                    var paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    { Direction = ParameterDirection.Output, Value = 0 };
                    cmd.Parameters.Add(paramEstatus);
                    var paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, -1)
                    { Direction = ParameterDirection.Output, Value = "" };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = Convert.ToInt32(paramEstatus.Value ?? 0);
                    string mensaje = (paramMensaje.Value ?? "").ToString().Trim();

                    if (estatus == 0)
                    {
                        MessageBox.Show("Guardado correctamente.", "Éxito",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BtnRefrescar_Click(sender, e);
                    }
                    else if (estatus == 1)
                    {
                        MostrarErroresJson(mensaje);
                    }
                    else
                    {
                        MessageBox.Show(mensaje, $"Error ({estatus})",
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConvertirTextoPlanoAHtml(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "<p></p>";

            string html = texto
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");

            string[] parrafos = html.Split(
                new[] { "\r\n\r\n", "\n\n", "\r\r" },
                StringSplitOptions.None
            );

            var sb = new StringBuilder();
            foreach (var p in parrafos)
            {
                string linea = p.Trim()
                    .Replace("\r\n", "<br/>")
                    .Replace("\n", "<br/>")
                    .Replace("\r", "<br/>");
                sb.AppendLine($"<p>{linea}</p>");
            }
            return sb.ToString();
        }

        private bool TieneEtiquetasHtml(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            var patron = new System.Text.RegularExpressions.Regex(
                @"<\/?[a-zA-Z][a-zA-Z0-9-]*(?:\s+[a-zA-Z0-9-]+(?:\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)*\s*\/?>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
            return patron.IsMatch(texto);
        }

        private bool ValidarEstructuraHtml(string html, out string mensajeError)
        {
            mensajeError = "";
            if (string.IsNullOrWhiteSpace(html))
                return true;

            var etiquetasAutoCierre = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "br", "img", "hr", "input", "meta", "link", "area", "base", "col", "embed",
                "source", "track", "wbr"
            };

            var pila = new System.Collections.Generic.Stack<string>();
            var matches = System.Text.RegularExpressions.Regex.Matches(
                html, @"<\/?([a-zA-Z][a-zA-Z0-9-]*)\b[^>]*>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                string etiqueta = m.Groups[1].Value.ToLower();
                bool esCierre = m.Value.StartsWith("</");
                bool terminaConBarra = m.Value.EndsWith("/>");

                if (terminaConBarra || etiquetasAutoCierre.Contains(etiqueta))
                    continue;

                if (esCierre)
                {
                    if (pila.Count == 0)
                    {
                        mensajeError = $"Etiqueta de cierre </{etiqueta}> sin apertura correspondiente.";
                        return false;
                    }
                    string ultima = pila.Pop();
                    if (ultima != etiqueta)
                    {
                        mensajeError = $"Se esperaba cierre de </{ultima}> pero se encontró </{etiqueta}>.";
                        return false;
                    }
                }
                else
                {
                    pila.Push(etiqueta);
                }
            }

            if (pila.Count > 0)
            {
                mensajeError = $"Faltan cerrar etiquetas: {string.Join(", ", pila)}";
                return false;
            }
            return true;
        }

        private string EscaparJson(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            return texto.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        private void MostrarErroresJson(string json)
        {
            try
            {
                var dt = ParseJsonErroresATabla(json);
                if (dt != null && dt.Rows.Count > 0)
                {
                    using (var frm = new FrmListaErroresExtendida(dt))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            catch
            {
                MessageBox.Show(json, "Errores de Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string ExtraerValorJson(string obj, string clave)
        {
            string buscar = $"\"{clave}\":";
            int idx = obj.IndexOf(buscar, StringComparison.Ordinal);
            if (idx < 0) return "";
            int ini = idx + buscar.Length;
            while (ini < obj.Length && char.IsWhiteSpace(obj[ini])) ini++;
            if (ini >= obj.Length) return "";

            if (obj[ini] == '"')
            {
                ini++;
                int fin = ini;
                while (fin < obj.Length && !(obj[fin] == '"' && obj[fin - 1] != '\\')) fin++;
                return fin < obj.Length ? obj.Substring(ini, fin - ini) : "";
            }
            else
            {
                int fin = ini;
                while (fin < obj.Length && obj[fin] != ',' && obj[fin] != '}') fin++;
                return obj.Substring(ini, fin - ini).Trim();
            }
        }


        // ? Parseo mejorado que llena TODAS las columnas
        private DataTable ParseJsonErroresATabla(string json)
        {
            var dt = new DataTable();
            dt.Columns.Add("secuencia", typeof(int));
            dt.Columns.Add("descripcion", typeof(string));
            dt.Columns.Add("codigo", typeof(string));
            dt.Columns.Add("mensaje", typeof(string));

            int sec = 1;
            int pos = 0;

            while ((pos = json.IndexOf("{", pos, StringComparison.Ordinal)) >= 0)
            {
                int fin = json.IndexOf("}", pos, StringComparison.Ordinal);
                if (fin < 0) break;

                string obj = json.Substring(pos, fin - pos + 1);

                string descripcion = ExtraerValorJson(obj, "descripcion") ?? "";
                string codigo = ExtraerValorJson(obj, "error")
                             ?? ExtraerValorJson(obj, "sec")
                             ?? "";
                string mensaje = ExtraerValorJson(obj, "mensaje")
                              ?? ExtraerValorJson(obj, "MensajeError")
                              ?? "";

                dt.Rows.Add(sec++, descripcion, codigo, mensaje);
                pos = fin + 1;
            }
            return dt;
        }

        private void BtnBaja_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtIdMotivo.Text))
            {
                MessageBox.Show("Seleccione un registro para eliminar.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombre = TxtDescripcion.Text.Trim();
            if (MessageBox.Show($"¿Eliminar el motivo?\n\n{nombre}",
                                "Confirmar Baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                int idMotivo = Convert.ToInt32(TxtIdMotivo.Text);

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("dbo.spd_conMotivosCorreoTbl", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PnIdMotivo", idMotivo);
                    cmd.Parameters.AddWithValue("@PsOperacion", _claveOperacion);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);

                    var paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    { Direction = ParameterDirection.Output, Value = 0 };
                    cmd.Parameters.Add(paramEstatus);
                    var paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 250)
                    { Direction = ParameterDirection.Output, Value = "" };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = Convert.ToInt32(paramEstatus.Value ?? 0);
                    string mensaje = (paramMensaje.Value ?? "").ToString().Trim();

                    if (estatus == 0)
                    {
                        MessageBox.Show("Eliminado correctamente.", "Baja Exitosa",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BtnRefrescar_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show(mensaje, $"Error ({estatus})",
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            CargarDatos();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            TxtIdMotivo.Text = "";
            TxtDescripcion.Text = "";
            TxtTitulo.Text = "";
            TxtCuerpo.Text = "";
            TxtHTML.Text = "";
            TxtURL.Text = "";
            TxtPerfilCorreo.Text = "";
            cmbEstatus.SelectedIndex = -1;
            cmbCompartir.SelectedIndex = -1;
            _hayRegistroSeleccionado = false;
            BtnBaja.Enabled = false;
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dgv == null || _dtOriginal == null) return;

                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"]
                              ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                string archivo = $"MotivosCorreo_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);

                var dtAnterior = _dgv.DataSource;
                _dgv.DataSource = _dtOriginal;

                Services.ExcelExportService.ExportarUsuarios(
                    ruta, _dgv, _claveOperacion, _nombreOperacion, _claveUsuario);

                _dgv.DataSource = dtAnterior;

                MessageBox.Show($"Exportado correctamente:\n{ruta}", "Exportación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtCuerpo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtHTML.Text))
            {
                TxtHTML.Text = ConvertirTextoPlanoAHtml(TxtCuerpo.Text);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
