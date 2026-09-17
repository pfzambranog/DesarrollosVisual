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
        private readonly string _claveOperacion;
        private readonly string _nombreOperacion;
        private readonly string _cadenaConexion;
        private DataGridView dgv;

        private DataTable _dtOriginal; // Guarda los valores al cargar


        // ✅ Constructor con 6 parámetros: recibe CLAVE + NOMBRE
        public FrmRelUsuarioOperacion(int idUsuario,         string claveUsuario,    DataTable dtPermisos,
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
            txtOperacion.Text = _claveOperacion;

            CargarLogo();

            // ✅ Al cambiar Usuario → recargar grid
            CmbUsuarios.SelectedIndexChanged += (s, args) =>
            {
                if (CmbUsuarios.SelectedValue != null && CmbUsuarios.SelectedIndex >= 0)
                    CargarGridPermisos();  // ✅ Solo llena si hay usuario seleccionado
                else
                {
                    dg.Columns.Clear();   // ✅ Limpia grid si no hay selección
                    dg.Rows.Clear();
                }
            };

            // ✅ Al cambiar filtros → actualizar SOLO si hay usuario
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

            // ✅ Cargar listas PERO SIN seleccionar usuario
            CargarUsuarios();
            CargarAutorizaciones();
            CargarEstatus();
            CmbUsuarios.SelectedIndex = -1; // ✅ SIN selección al iniciar

            AjusteGrid();
            dg.Columns.Clear(); // ✅ Grid VACÍO al iniciar
            dg.Rows.Clear();

            // ✅ Permiso para botón Procesar
            bool tienePermisoProcesar = _dtPermisos.AsEnumerable()
                 .Any(f => f["claveOperacion"].ToString().Trim() == _claveOperacion.Trim()
                       && Convert.ToInt32(f["idAutorizacion"]) >= 4);
            BtnProcesar.Enabled = tienePermisoProcesar;
        }


        private void CargarGridPermisos()
        {
            // ✅ Validar usuario seleccionado
            if (CmbUsuarios.SelectedValue == null || !(CmbUsuarios.SelectedValue is int IdUsuarioSel))
            {
                dg.Columns.Clear();
                dg.Rows.Clear();
                return; // ✅ DETENER ejecución si NO hay usuario seleccionado
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
                    if (valor >= 0) filtroAut = valor;
                }
                // ✅ Leer Estatus: -1 = "(Todos)" → SIN filtro
                int? filtroEst = null;
                if (cmbEstatus.SelectedValue != null)
                {
                    int valor = Convert.ToInt32(cmbEstatus.SelectedValue);
                    if (valor >= 0) filtroEst = valor;
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
                     And    b.idUsuario   = d.idUsuario)
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
       AND b.idAutorizacion >= 0  
INNER JOIN dbo.segUsuariosTbl d 
        ON d.idUsuario = b.idUsuario
WHERE  a.idEstatus = 1
ORDER BY Operacion, idAutorizacion";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuarioSel); // ✅ Ahora SÍ existe en este ámbito
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
                        // 🔍 Filtro por Autorización
                        if (filtroAut.HasValue)
                        {
                            string parte = $" idAutorizacion = {filtroAut.Value} ";
                            condicion = condicion == "" ? parte : condicion + " AND " + parte;
                        }
                        // 🔍 Filtro por Estatus
                        if (filtroEst.HasValue)
                        {
                            string parte = $" idEstatus = {filtroEst.Value} ";
                            condicion = condicion == "" ? parte : condicion + " AND " + parte;
                        }

                        // ✅ Aplicar filtro
                        if (!string.IsNullOrWhiteSpace(condicion))
                        {
                            dt.DefaultView.RowFilter = condicion;
                        }

                        dg.DataError += (s, e) => { e.ThrowException = false; };

                        ConfigurarAparienciaGrid();

                        _dtOriginal = dt.DefaultView.ToTable().Copy();

                        ConfigurarColumnasEditables(dt.DefaultView.ToTable()); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\nFiltro: {txtFiltroOperaciones.Text}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ Al terminar edición → verificar si hay cambios
        private void dg_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            VerificarCambios();
        }

        // ✅ Compara grid vs valores originales → habilita botón si hay diferencias
        private void VerificarCambios()
        {
            if (_dtOriginal == null)
            {
                BtnProcesar.Enabled = false;
                return;
            }

            bool hayCambios = false;

            foreach (DataGridViewRow filaGrid in dg.Rows)
            {
                int idOperacion = Convert.ToInt32(filaGrid.Cells["idOperacion"].Value);
                int idUsuario = Convert.ToInt32(filaGrid.Cells["idUsuario"].Value);

                // Buscar fila original correspondiente
                DataRow[] filasOriginal = _dtOriginal.Select(
                    $"idOperacion = {idOperacion} AND idUsuario = {idUsuario}");

                if (filasOriginal.Length > 0)
                {
                    int autOriginal = Convert.ToInt32(filasOriginal[0]["idAutorizacion"]);
                    int estOriginal = Convert.ToInt32(filasOriginal[0]["idEstatus"]);

                    int autActual = Convert.ToInt32(filaGrid.Cells["idAutorizacion"].Value);
                    int estActual = Convert.ToInt32(filaGrid.Cells["idEstatus"].Value);

                    if (autActual != autOriginal || estActual != estOriginal)
                    {
                        hayCambios = true;
                        break;
                    }
                }
            }

            BtnProcesar.Enabled = hayCambios;
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

        private void ConfigurarColumnasEditables(DataTable dt) // ✅ Recibe los datos
        {
            // =====================================================
            // ✅ PASO 1: Eliminar TODAS las columnas automáticas
            // =====================================================
            dg.Columns.Clear();

            // =====================================================
            // ✅ PASO 2: Crear columna OCULTA para idUsuario
            // =====================================================
            dg.Columns.Add("idUsuario", "idUsuario");
            dg.Columns["idUsuario"].Visible = false;

            // =====================================================
            // ✅ PASO 3: Crear columna OCULTA para idOperacion
            // =====================================================
            dg.Columns.Add("idOperacion", "idOperacion");
            dg.Columns["idOperacion"].Visible = false;

            // =====================================================
            // ✅ PASO 4: Crear columna OCULTA para ClaveOperacion
            // =====================================================
            dg.Columns.Add("ClaveOperacion", "ClaveOperacion");
            dg.Columns["ClaveOperacion"].Visible = false;

            // =====================================================
            // ✅ PASO 5: Crear columna Operación (SOLO LECTURA)
            // =====================================================
            dg.Columns.Add("Operacion", "Operación");
            dg.Columns["Operacion"].Width = 400;
            dg.Columns["Operacion"].ReadOnly = true;
            dg.Columns["Operacion"].DefaultCellStyle.BackColor = Color.LightGray;
            dg.Columns["Operacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // =====================================================
            // ✅ PASO 6: Crear columna COMBO — AUTORIZACIÓN
            // =====================================================
            DataGridViewComboBoxColumn colAut = new DataGridViewComboBoxColumn
            {
                Name = "idAutorizacion",
                HeaderText = "Autorización",
                Width = 210,
                ReadOnly = false,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Flat
            };

            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT valor, CONCAT(valor, ' - ', descripcion) AS descripcion " +
                "FROM   dbo.catGeneralesTbl " +
                "WHERE  tabla = 'segAutOperacionesTbl' " +
                "AND    columna = 'idAutorizacion' " +
                "ORDER BY valor", cn))
            {
                cn.Open();
                DataTable dtComboAut = new DataTable(); // ✅ NOMBRE DIFERENTE
                dtComboAut.Load(cmd.ExecuteReader());
                colAut.DataSource = dtComboAut;
                colAut.ValueMember = "valor";
                colAut.DisplayMember = "descripcion";
            }
            dg.Columns.Add(colAut);

            // =====================================================
            // ✅ PASO 7: Crear columna COMBO — ESTATUS
            // =====================================================
            DataGridViewComboBoxColumn colEst = new DataGridViewComboBoxColumn
            {
                Name = "idEstatus",
                HeaderText = "Estatus",
                Width = 210,
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
                DataTable dtComboEst = new DataTable(); // ✅ NOMBRE DIFERENTE
                dtComboEst.Load(cmd.ExecuteReader());
                colEst.DataSource = dtComboEst;
                colEst.ValueMember = "valor";
                colEst.DisplayMember = "descripcion";
            }
            dg.Columns.Add(colEst);

            // =====================================================
            // ✅ PASO 8: Copiar datos del DataTable a las columnas MANUALES
            // =====================================================
            dg.Rows.Clear();
            foreach (DataRow fila in dt.Rows) // ⚠️ Cambia "dt" por el nombre de tu DataTable si es diferente
            {
                int indice = dg.Rows.Add();
                DataGridViewRow filaGrid = dg.Rows[indice];

                filaGrid.Cells["idUsuario"].Value = fila["idUsuario"];
                filaGrid.Cells["idOperacion"].Value = fila["idOperacion"];
                filaGrid.Cells["ClaveOperacion"].Value = fila["ClaveOperacion"];
                filaGrid.Cells["Operacion"].Value = fila["Operacion"];
                filaGrid.Cells["idAutorizacion"].Value = fila["idAutorizacion"]; // ✅ Valor numérico → combo muestra texto
                filaGrid.Cells["idEstatus"].Value = fila["idEstatus"]; // ✅ Valor numérico → combo muestra texto
            }

            // =====================================================
            // ✅ PASO 9: Alineaciones visuales
            // =====================================================
            dg.Columns["Operacion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dg.Columns["Operacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dg.Columns["idAutorizacion"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["idAutorizacion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dg.Columns["idEstatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dg.Columns["idEstatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ✅ Conectar evento para detectar cambios
            dg.CellEndEdit += dg_CellEndEdit;
        }

        // ✅ Permite que el combo se abra y se edite correctamente
        private void dg_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox cbo)
            {
                cbo.DropDownStyle = ComboBoxStyle.DropDownList; // ✅ Solo seleccionar de lista
            }
        }

        private void dg_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            var col = dg.Columns[e.ColumnIndex];
            if (col.Name == "idAutorizacion" || col.Name == "idEstatus")
            {
                // El valor ya viene como número → lo usamos directamente
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    e.ParsingApplied = true;
                }
            }
        }

        // ✅ Al mostrar la celda → convertir número a su texto descriptivo
        private void dg_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            var col = dg.Columns[e.ColumnIndex];
            if ((col.Name == "idAutorizacion" || col.Name == "idEstatus") && e.Value != null)
            {
                // El valor numérico se mantiene internamente, se muestra el texto del combo
                e.FormattingApplied = false;
            }
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

        // ✅ Al mostrar la celda → asignar valor inicial al combo
        private void dg_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dg.Rows)
            {
                // ✅ Autorización: asignar valor numérico al combo
                if (fila.Cells["idAutorizacion"] != null && fila.Cells["idAutorizacion"].Value != null)
                {
                    int valorAut = Convert.ToInt32(fila.Cells["idAutorizacion"].Value);
                    fila.Cells["idAutorizacion"].Value = valorAut;
                }

                // ✅ Estatus: asignar valor numérico al combo
                if (fila.Cells["idEstatus"] != null && fila.Cells["idEstatus"].Value != null)
                {
                    int valorEst = Convert.ToInt32(fila.Cells["idEstatus"].Value);
                    fila.Cells["idEstatus"].Value = valorEst;
                }
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
            try
            {
                // ✅ PASO A: Construir JSON SOLO con filas MODIFICADAS
                StringBuilder json = new StringBuilder();
                json.Append("[");
                bool primero = true;

                foreach (DataGridViewRow filaGrid in dg.Rows)
                {
                    int idOperacion = Convert.ToInt32(filaGrid.Cells["idOperacion"].Value);
                    int idUsuarioFila = Convert.ToInt32(filaGrid.Cells["idUsuario"].Value);
                    int autActual = Convert.ToInt32(filaGrid.Cells["idAutorizacion"].Value);
                    int estActual = Convert.ToInt32(filaGrid.Cells["idEstatus"].Value);

                    // Buscar valor original
                    DataRow[] filasOriginal = _dtOriginal?.Select(
                        $"idOperacion = {idOperacion} AND idUsuario = {idUsuarioFila}");

                    bool esModificada = true;
                    if (filasOriginal != null && filasOriginal.Length > 0)
                    {
                        int autOriginal = Convert.ToInt32(filasOriginal[0]["idAutorizacion"]);
                        int estOriginal = Convert.ToInt32(filasOriginal[0]["idEstatus"]);
                        esModificada = (autActual != autOriginal || estActual != estOriginal);
                    }

                    if (esModificada)
                    {
                        if (!primero) json.Append(",");
                        json.Append($"{{\"IdUsuario\":{idUsuarioFila}," +
                                          $"\"IdOperacion\":{idOperacion}," +
                                          $"\"IdAutorizacion\":{autActual}," +
                                          $"\"IdEstatus\":{estActual}}}");
                        primero = false;
                    }
                }
                json.Append("]");

                string jsonDatos = json.ToString();

                // ✅ Si no hay nada que procesar
                if (jsonDatos == "[]")
                {
                    MessageBox.Show("No hay cambios para procesar.", "Información",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ PASO B: Llamar al Procedimiento Almacenado
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("Spp_segAutOperacionesTbl", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // ✅ Parámetros de ENTRADA
                        cmd.Parameters.AddWithValue("@PsJasonIn", jsonDatos);
                        cmd.Parameters.AddWithValue("@PsOperacion", txtOperacion.Text);
                        cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);
                        cmd.Parameters.AddWithValue("@PsIpAct", DBNull.Value);
                        cmd.Parameters.AddWithValue("@PsMacAddressAct", DBNull.Value);

                        // ✅ Parámetros de SALIDA — Tamaño MAX para mensajes completos
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

                        // ✅ PASO C: Leer y evaluar resultado
                        int estatus = paramEstatus.Value == DBNull.Value ? 9999 : Convert.ToInt32(paramEstatus.Value);
                        string mensaje = paramMensaje.Value?.ToString()?.Trim() ?? "";

                        if (estatus == 0)
                        {
                            MessageBox.Show("✅ Actualización realizada correctamente.",
                                            "Procesamiento Exitoso",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarGridPermisos();
                            BtnProcesar.Enabled = false;
                        }
                        else if (estatus == 1)
                        {
                            // ✅ Estatus = 1 → JSON con lista de errores → abrir pantalla
                            var dtErrores = ParsearErroresJson(mensaje);
                            if (dtErrores != null && dtErrores.Rows.Count > 0)
                            {
                                string rutaLogo = Path.Combine(Application.StartupPath, ConfigurationManager.AppSettings["Imagenes"] ?? "", "Logo.png");
                                using (FrmListaErrores frmErr = new FrmListaErrores(dtErrores, rutaLogo))
                                {
                                    frmErr.ShowDialog(this);
                                    // ✅ Al cerrar → NO refrescar la pantalla principal
                                }
                            }
                            else
                            {
                                MessageBox.Show($"⚠️ No se pudo interpretar la lista de errores:\n{mensaje}",
                                                "Errores de Validación",
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"⚠️ Advertencia / Información:\n\n{mensaje}",
                                            "Procesamiento",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado:\n{ex.Message}",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRefrescar_Click(object sender, EventArgs e)
        {
            txtFiltroOperaciones.Text = "";
            if (cmbAutorizaciones.Items.Count > 0) cmbAutorizaciones.SelectedIndex = 0;
            if (cmbEstatus.Items.Count > 0) cmbEstatus.SelectedIndex = 0;
            CmbUsuarios.SelectedIndex = -1;
            dg.Columns.Clear();
            dg.Rows.Clear();

            // ✅ Limpiar valores originales
            _dtOriginal?.Clear();
            BtnProcesar.Enabled = false;
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
        // ✅ Convierte el JSON de errores en un DataTable legible
        private DataTable ParsearErroresJson(string jsonTexto)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Secuencia", typeof(int));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Operación", typeof(string));
            dt.Columns.Add("Autorización", typeof(int));
            dt.Columns.Add("Estatus", typeof(int));
            dt.Columns.Add("N° Error", typeof(int));
            dt.Columns.Add("Mensaje", typeof(string));

            if (string.IsNullOrWhiteSpace(jsonTexto))
                return dt;

            try
            {
                // ✅ Quitar corchetes externos
                string contenido = jsonTexto.Trim().Trim('[', ']');
                if (string.IsNullOrWhiteSpace(contenido))
                    return dt;

                // ✅ Separar cada objeto
                string[] objetos = contenido.Split(new[] { "}," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string obj in objetos)
                {
                    string fila = obj.Trim().Trim('{', '}') + "}";

                    dt.Rows.Add(
                        LeerValorInt(fila, "secuencia") ?? 0,
                        LeerValorCadena(fila, "usuario"),
                        LeerValorCadena(fila, "operacion"),
                        LeerValorInt(fila, "idAutorizacion") ?? 0,
                        LeerValorInt(fila, "idEstatus") ?? 0,
                        LeerValorInt(fila, "error") ?? 0,
                        LeerValorCadena(fila, "mensaje")
                    );
                }
            }
            catch { /* Ignorar errores de parseo */ }

            return dt;
        }

        // ✅ Leer entero desde fragmento JSON
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

        // ✅ Leer cadena desde fragmento JSON
        private string LeerValorCadena(string texto, string campo)
        {
            string p = $"\"{campo}\":\"";
            int i = texto.IndexOf(p);
            if (i < 0) return "";
            int f = texto.IndexOf('"', i + p.Length);
            return f < 0 ? "" : texto.Substring(i + p.Length, f - i - p.Length).Replace("\\\"", "\"");
        }


        private void panel1_Paint(object sender, PaintEventArgs e) { }
      //  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pnlBarrainicial_Paint(object sender, PaintEventArgs e) { }
    }
}
