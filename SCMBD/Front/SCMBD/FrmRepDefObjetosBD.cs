using SCMBD.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmRepDefObjetosBD : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;

        public FrmRepDefObjetosBD(int idUsuario, string claveUsuario, DataTable dtPermisos,
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

        private void FrmRepDefObjetosBD_Load(object sender, EventArgs e)
        {
            this.Text = _nombreOperacion;
            txtUsuario.Text = _claveUsuario;
            txtOperacion.Text = _claveOperacion;
            CargarLogo();
            CargarReportes();
            ConfigurarGrid();
            BtnExcel.Enabled = false;
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
WHERE criterio = 'repobjbd'
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
            dg.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg.BorderStyle = BorderStyle.Fixed3D;
            dg.RowHeadersVisible = false;
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg.AllowUserToAddRows = false;
            dg.MultiSelect = false;

            // ✅ Ajuste automático de columnas Y filas
            dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // ← Clave: altura por contenido
            dg.DefaultCellStyle.WrapMode = DataGridViewTriState.True;   // ← Saltos de línea permitidos
            dg.RowTemplate.Height = 25; // Altura mínima base
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

                    // ✅ PRIMERO leemos el resultado del sp (las filas)
                    DataTable dt = new DataTable();
                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        dt.Load(lector);
                    }

                    // ✅ AHORA leemos los parámetros de salida
                    int estatus = Convert.ToInt32(pEstatus.Value);
                    string mensaje = pMensaje.Value?.ToString()?.Trim() ?? "";

                    // ✅ Si hay filas → las mostramos directamente
                    if (dt.Rows.Count > 0)
                    {
                        dg.DataSource = dt;
                        AplicarEncabezadosAmigables();
                        BtnExcel.Enabled = true;
                    }
                    // ✅ Si no hay filas pero hay mensaje JSON → objetos con errores
                    else if (!string.IsNullOrWhiteSpace(mensaje) && mensaje.StartsWith("["))
                    {
                        dt = ParsearJsonADataTable(mensaje);
                        if (dt.Rows.Count > 0)
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
                        MessageBox.Show("No se encontraron resultados.",
                                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Error al procesar los resultados JSON.", "Error",
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
                string nombre = col.Name.ToLower().Trim(); // ✅ TODO a minúsculas

                // Encabezados SIEMPRE centrados
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // ===== COMUNES =====
                if (nombre == "basededatos" || nombre == "base de datos")
                {
                    col.HeaderText = "Base de Datos";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ===== FUNCIONES =====
                else if (nombre == "esquema")
                {
                    col.HeaderText = "Esquema";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "nombrefuncion") 
                {
                    col.HeaderText = "Nombre Función";
                    col.FillWeight = 22; 
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "tipofuncion")
                {
                    col.HeaderText = "Tipo Función";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "descripciontipo")
                {
                    col.HeaderText = "Descripción Tipo";
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "idparametro")
                {
                    col.HeaderText = "Id Parámetro";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "nombreparametro")
                {
                    col.HeaderText = "Nombre Parámetro";
                    col.FillWeight = 20;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "tipodato")
                {
                    col.HeaderText = "Tipo Dato";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "longitud")
                {
                    col.HeaderText = "Longitud";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "tipoparametrofn") // ✅ minúsculas
                {
                    col.HeaderText = "Tipo Parámetro";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ===== PROCEDIMIENTOS =====
                else if (nombre == "procedimiento")
                {
                    col.HeaderText = "Procedimientos";
                    col.FillWeight = 15;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "posicion")
                {
                    col.HeaderText = "Pos";
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "tipoparametro")
                {
                    col.HeaderText = "Tipo Parámetro";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "precisionnumerica")
                {
                    col.HeaderText = "Precisión";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "collation")
                {
                    {
                        col.HeaderText = "Intercalación";
                        col.FillWeight = 12;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                // ===== TABLAS 

                else if (nombre == "tabla")
                {
                    col.HeaderText = "Tabla";
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "columna")
                {
                    col.HeaderText = "Columna";
                    col.FillWeight = 20;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "tipodedato")
                {
                    col.HeaderText = "Tipo de Dato";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "requerido")
                {
                    col.HeaderText = "Requerido";
                    col.FillWeight = 7;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "llaveprimaria")
                {
                    col.HeaderText = "PK";
                    col.FillWeight = 5;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "descripcion")
                {
                    col.HeaderText = "Descripción";
                    col.FillWeight = 25;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                // ===== Vistas

                else if (nombre == "vista")
                {
                    col.HeaderText = "Vista";
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "columnavw")
                {
                    col.HeaderText = "Columnas";
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "tipodatovw")
                {
                    col.HeaderText = "Tipo de Datos";
                    col.FillWeight = 10;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                else if (nombre == "dependencias")
                {
                    col.HeaderText = "Dependencias";
                    col.FillWeight = 35;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "nombretabla")
                {
                    col.HeaderText = "Tabla";
                    col.FillWeight = 18;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "nombretrigger")
                {
                    col.HeaderText = "Nombre Trigger";
                    col.FillWeight = 22;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else if (nombre == "propietario")
                {
                    col.Visible = false;
                    continue;
                }
                else if (nombre == "habilitado")
                {
                    col.HeaderText = "Habilitado";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "insteadof")
                {
                    col.HeaderText = "InsteadOf";
                    col.FillWeight = 8;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "after")
                {
                    col.HeaderText = "After";
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "insert")
                {
                    col.HeaderText = "Insert";
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "update")
                {
                    col.HeaderText = "Update";
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (nombre == "delete")
                {
                    col.HeaderText = "Delete";
                    col.FillWeight = 6;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ===== POR DEFECTO =====
                else
                {
                    col.HeaderText = col.Name;
                    col.FillWeight = 12;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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
                string tituloReporte = CmbReportes.Text.Trim();

                Services.ExcelExportService.ExportarUsuarios(
                    ruta, dg, _claveOperacion, tituloReporte, _claveUsuario);

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

        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }


        private void BtnVerScript_Click(object sender, EventArgs e)
        {
            if (dg.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un objeto del grid.", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow fila = dg.SelectedRows[0];

            // ==============================================
            // 1. ESQUEMA
            // ==============================================
            string esquema = LeerValorCelda(fila, "Esquema");
            if (string.IsNullOrWhiteSpace(esquema))
                esquema = "dbo";

            // ==============================================
            // 2. DETECTAR NOMBRE Y TIPO — TODOS
            // ==============================================
            string nombreObjeto = null;
            string tipoObjeto = null;

            // Procedimientos
            string valProc = LeerValorCelda(fila, "Procedimiento");
            if (!string.IsNullOrWhiteSpace(valProc))
            {
                nombreObjeto = valProc;
                tipoObjeto = "Procedimiento";
            }

            // ✅ FUNCIONES — nombre real: NombreFuncion (sin espacio)
            string valFunc = LeerValorCelda(fila, "NombreFuncion");
            if (!string.IsNullOrWhiteSpace(valFunc) && string.IsNullOrWhiteSpace(nombreObjeto))
            {
                nombreObjeto = valFunc;
                tipoObjeto = "Función";
            }

            // Vistas
            string valVista = LeerValorCelda(fila, "Vista");
            if (!string.IsNullOrWhiteSpace(valVista) && string.IsNullOrWhiteSpace(nombreObjeto))
            {
                nombreObjeto = valVista;
                tipoObjeto = "Vista";
            }

            // Triggers
            string valTrigger = LeerValorCelda(fila, "Nombre Trigger");
            if (!string.IsNullOrWhiteSpace(valTrigger) && string.IsNullOrWhiteSpace(nombreObjeto))
            {
                nombreObjeto = valTrigger;
                tipoObjeto = "Trigger";
            }

            // Tablas
            string valTabla = LeerValorCelda(fila, "Nombre Tabla");
            if (!string.IsNullOrWhiteSpace(valTabla) && string.IsNullOrWhiteSpace(nombreObjeto))
            {
                nombreObjeto = valTabla;
                tipoObjeto = "Tabla";
            }

            // ==============================================
            // 3. VALIDAR
            // ==============================================
            if (string.IsNullOrWhiteSpace(nombreObjeto))
            {
                MessageBox.Show(
                    "No se pudo identificar el objeto seleccionado.",
                    "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ==============================================
            // 4. GENERAR SCRIPT
            // ==============================================
            string rutaArchivo = Services.ScriptsExportService.ExportarDefinicionObjeto(
                esquema,
                nombreObjeto,
                tipoObjeto,
                _cadenaConexion,
                _claveOperacion,
                _idUsuario);

            if (!string.IsNullOrWhiteSpace(rutaArchivo))
            {
                MessageBox.Show($"✅ Script generado:\n{rutaArchivo}",
                                "Archivo Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Services.ScriptsExportService.AbrirArchivo(rutaArchivo);
            }
        }

        private string LeerValorCelda(DataGridViewRow fila, string nombreColumna)
        {
            if (fila == null) return "";
            foreach (DataGridViewColumn col in dg.Columns)
            {
                if (string.Equals(col.Name, nombreColumna, StringComparison.OrdinalIgnoreCase))
                {
                    return fila.Cells[col.Index].Value?.ToString()?.Trim() ?? "";
                }
            }
            return "";
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
