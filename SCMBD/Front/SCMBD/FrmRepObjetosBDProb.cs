using SCMBD.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmRepObjetosBDProb : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;

        public FrmRepObjetosBDProb(int idUsuario, string claveUsuario, DataTable dtPermisos,
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

        private void FrmRepObjetosBDProb_Load(object sender, EventArgs e)
        {
            this.Text = _nombreOperacion;
            txtUsuario.Text = _claveUsuario;
            txtOperacion.Text = _claveOperacion;
            CargarLogo();
            CargarReportes();
            ConfigurarGrid();
            BtnExcel.Enabled = false;

            // 
            dg.CellDoubleClick += dg_CellDoubleClick;
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void CargarReportes()
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                string sql = @"
SELECT -1 AS valor, ' ' AS descripcion, ' ' AS valorAdicional
UNION ALL
SELECT valor, descripcion, valorAdicional
FROM dbo.catCriteriosTbl
WHERE criterio = 'repmant'
ORDER BY valor";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    CmbReportes.DisplayMember = "descripcion";
                    CmbReportes.ValueMember = "valorAdicional";
                    CmbReportes.DataSource = dt;
                    CmbReportes.SelectedIndex = 0;
                }
            }
        }

        private void ConfigurarGrid()
        {
            dg.Columns.Clear();
            dg.EnableHeadersVisualStyles = false;
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dg.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.BorderStyle = BorderStyle.Fixed3D;
            dg.RowHeadersVisible = false;
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg.AllowUserToAddRows = false;
            dg.MultiSelect = false;
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CmbReportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dg.DataSource = null;
            BtnExcel.Enabled = false;

            if (CmbReportes.SelectedValue == null ||
                CmbReportes.SelectedValue.ToString().Trim() == "")
                return;

            string spNombre = CmbReportes.SelectedValue.ToString().Trim();
            EjecutarReporte(spNombre);
        }

        private void EjecutarReporte(string spNombre)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand(spNombre, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsOperacion", _claveOperacion);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);

                    SqlParameter pEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(pEstatus);

                    SqlParameter pMensaje = new SqlParameter("@PsMensaje", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output,
                        Value = DBNull.Value
                    };
                    cmd.Parameters.Add(pMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    int estatus = Convert.ToInt32(pEstatus.Value);
                    string mensaje = pMensaje.Value?.ToString()?.Trim() ?? "";

                    if (estatus == 0)
                    {
                        MessageBox.Show("✅ No se encontraron objetos con problemas.",
                                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (estatus == 1)
                    {
                        DataTable dt = ParsearJsonADataTable(mensaje);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dg.DataSource = dt;
                            AplicarEncabezadosAmigables();
                            BtnExcel.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron objetos con problemas.",
                                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"⚠️ {mensaje}\nCódigo: {estatus}",
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ejecutar:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ParsearJsonADataTable(string jsonTexto)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrWhiteSpace(jsonTexto)) return dt;

            try
            {
                string contenido = jsonTexto.Trim().Trim('[', ']');
                if (string.IsNullOrWhiteSpace(contenido)) return dt;

                string[] registros = contenido.Split(new string[] { "}," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string reg in registros)
                {
                    string limpio = reg.Trim().Trim('{', '}');
                    var campos = ExtraerCampos(limpio);

                    if (dt.Columns.Count == 0)
                    {
                        foreach (var kv in campos)
                            dt.Columns.Add(kv.Key, typeof(string));
                    }

                    DataRow fila = dt.NewRow();
                    foreach (var kv in campos)
                    {
                        if (dt.Columns.Contains(kv.Key))
                            fila[kv.Key] = kv.Value ?? "";
                    }
                    dt.Rows.Add(fila);
                }
            }
            catch
            {
                MessageBox.Show("Error al procesar los resultados.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return dt;
        }

        private Dictionary<string, string> ExtraerCampos(string texto)
        {
            var res = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(texto)) return res;

            int pos = 0;
            while (pos < texto.Length)
            {
                int iClave = texto.IndexOf("\"", pos);
                if (iClave < 0) break;
                int fClave = texto.IndexOf("\"", iClave + 1);
                if (fClave < 0) break;
                string clave = texto.Substring(iClave + 1, fClave - iClave - 1);

                int iValor = texto.IndexOf(":", fClave + 1);
                if (iValor < 0) break;
                iValor++;

                string valor;
                if (iValor < texto.Length && texto[iValor] == '"')
                {
                    int fValor = texto.IndexOf("\"", iValor + 1);
                    if (fValor < 0) break;
                    valor = texto.Substring(iValor + 1, fValor - iValor - 1).Replace("\\\"", "\"");
                    pos = fValor + 1;
                }
                else
                {
                    int fValor = texto.IndexOf(",", iValor);
                    if (fValor < 0) fValor = texto.Length;
                    valor = texto.Substring(iValor, fValor - iValor).Trim();
                    pos = fValor;
                }
                res[clave] = valor;
            }
            return res;
        }


        private void AplicarEncabezadosAmigables()
        {
            foreach (DataGridViewColumn col in dg.Columns)
            {
                string nombre = col.Name.ToLower();

                // ✅ Ocultar columna secuencia
                if (nombre == "secuencia")
                {
                    col.Visible = false;
                    continue;
                }

                // ✅ Encabezados centrados en TODAS las columnas
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                switch (nombre)
                {
                    case "esquema":
                        col.HeaderText = "Esquema";
                        col.Width = 90;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "nombreobjeto":
                    case "nombreobj":
                        col.HeaderText = "Objeto";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.FillWeight = 90; 
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;

                    case "tipoobjeto":
                        col.HeaderText = "Tipo";
                        col.Width = 120;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "objetoreferenciado":
                        col.HeaderText = "Referencia";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.FillWeight = 40;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;

                    case "detalleerror":
                        col.HeaderText = "Detalle del Problema";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.FillWeight = 120;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;

                    case "mensajeerror":
                        col.HeaderText = "Mensaje de Error";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        col.FillWeight = 130;  // ✅ Incrementada con el espacio liberado
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;

                    default:
                        col.HeaderText = col.Name;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;
                }
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dg == null || dg.Rows.Count == 0) return;

                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"] ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string archivo = $"ReporteObjetosBD_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);

                // ✅ Título = nombre del reporte seleccionado en el combo
                string tituloReporte = CmbReportes.Text.Trim();

                // ✅ Ocultar columna secuencia SOLO para la exportación
                bool secuenciaEraVisible = true;
                if (dg.Columns.Contains("secuencia"))
                {
                    secuenciaEraVisible = dg.Columns["secuencia"].Visible;
                    dg.Columns["secuencia"].Visible = false;
                }
                if (dg.Columns.Contains("Secuencia"))
                {
                    secuenciaEraVisible = dg.Columns["Secuencia"].Visible;
                    dg.Columns["Secuencia"].Visible = false;
                }

                // ✅ Llamar al servicio con el DataGridView (compatibilidad con tu servicio)
                Services.ExcelExportService.ExportarUsuarios(
                    ruta, dg, _claveOperacion, tituloReporte, _claveUsuario);

                // ✅ Restaurar visibilidad original en pantalla
                if (dg.Columns.Contains("secuencia"))
                    dg.Columns["secuencia"].Visible = secuenciaEraVisible;
                if (dg.Columns.Contains("Secuencia"))
                    dg.Columns["Secuencia"].Visible = secuenciaEraVisible;

                MessageBox.Show($"✅ Exportado:\n{ruta}", "Excel",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exportando:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnProcesar_Click(object sender, EventArgs e)
        {
            if (CmbReportes.SelectedValue == null ||
                CmbReportes.SelectedValue.ToString().Trim() == "")
            {
                MessageBox.Show("Seleccione un reporte de la lista.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombreSP = CmbReportes.SelectedValue.ToString().Trim();
            EjecutarReporte(nombreSP);
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            CmbReportes.SelectedIndex = 0;
            dg.DataSource = null;
            BtnExcel.Enabled = false;
        }


        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e)
        {
            // Evento requerido por diseñador — sin lógica
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Evento requerido por diseñador — sin lógica
        }
        private void BtnVerScript_Click(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un objeto del grid.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow fila = dg.SelectedRows[0];

            // ✅ Leer por posición: 0=Esquema, 1=Objeto, 2=Tipo
            string esquema = fila.Cells[0].Value?.ToString()?.Trim() ?? "";
            string nombreObjeto = fila.Cells[1].Value?.ToString()?.Trim() ?? "";
            string tipoObjeto = fila.Cells[2].Value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(esquema) || string.IsNullOrWhiteSpace(nombreObjeto))
            {
                MessageBox.Show("No se pudieron leer los datos del objeto seleccionado.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Llamar con TODOS los parámetros del estándar
            string rutaArchivo = Services.ScriptsExportService.ExportarDefinicionObjeto(
                esquema,
                nombreObjeto,
                tipoObjeto,
                _cadenaConexion,
                _claveOperacion,   // ← Clave como SU1012
                _idUsuario);    // ← ID del usuario en sesión

            if (!string.IsNullOrWhiteSpace(rutaArchivo))
            {
                MessageBox.Show($"✅ Script generado:\n{rutaArchivo}",
                                "Archivo Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Services.ScriptsExportService.AbrirArchivo(rutaArchivo);
            }
        }

        private void dg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnVerScript_Click(sender, e);
            }
        }

    }
}
