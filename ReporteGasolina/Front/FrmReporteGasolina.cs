using ReporteGasolina.Infrastructure;
using ReporteGasolina.Models;
using ReporteGasolina.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;



namespace ReporteGasolina
{

    public partial class FrmReporteGasolina : Form
    {
        private readonly GasolinaService _gasolinaService;

        private readonly ExcelGasolinaService _excelService;

        private readonly ExcelExportService  _excelExportService;

        private readonly ReporteGasolinaService _reporteService;

        private readonly ExcelReporteGasolinaService _excelReporteGasolinaService;

        private void FrmReporteGasolina_Load(object sender, EventArgs e)
        {
            CargarMeses();

            CargarPeriodoActual();

            ConsultarPrecios();

        }


  
        public FrmReporteGasolina()
        {
            InitializeComponent();

            _gasolinaService = new GasolinaService();
            
           _excelService = new ExcelGasolinaService();

           _excelExportService = new ExcelExportService();

           _reporteService = new ReporteGasolinaService();
     
           _excelReporteGasolinaService = new ExcelReporteGasolinaService();


            this.Text = "Reporte de Gasolina";
            this.WindowState = FormWindowState.Maximized;

            txtFechaProceso.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            txtOperacion.Text = AppSettings.Operacion;
            txtUsuario.Text = AppSettings.Usuario;


            toolTip1.SetToolTip(btnConsultar, "Consultar precios de gasolina");
            toolTip1.SetToolTip(btnImportarExcel, "Importar precios desde Excel");

            toolTip1.SetToolTip(btnExcel,"Exportar a Excel");

            toolTip1.SetToolTip(btnProcesar, "Procesar reporte mensual");

            toolTip1.SetToolTip(btnSalir, "Salir del sistema");

            try
            {
                CargarMeses();
                CargarPeriodoActual();
                ConsultarPrecios();
            }
            catch (Exception ex)
            {
            MessageBox.Show(
                    ex.Message,
                    "Error al iniciar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }

        private void FormatearGridReporte()
        {
            dvgPrecios.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dvgPrecios.ReadOnly = true;

            if (dvgPrecios.Columns["Region"] != null)
                dvgPrecios.Columns["Region"].HeaderText = "Región";

            if (dvgPrecios.Columns["DepZona"] != null)
                dvgPrecios.Columns["DepZona"].HeaderText = "Dep-Zona";

            if (dvgPrecios.Columns["trabajador"] != null)
                dvgPrecios.Columns["trabajador"].HeaderText = "Trabajador";

            if (dvgPrecios.Columns["nombre"] != null)
                dvgPrecios.Columns["nombre"].HeaderText = "Nombre";

            if (dvgPrecios.Columns["nss"] != null)
                dvgPrecios.Columns["nss"].HeaderText = "NSS";

            if (dvgPrecios.Columns["Ciudad"] != null)
                dvgPrecios.Columns["Ciudad"].HeaderText = "Ciudad";

            if (dvgPrecios.Columns["pvpLitro"] != null)
                dvgPrecios.Columns["pvpLitro"].HeaderText = "PVP Ciudad";

            if (dvgPrecios.Columns["cantLitros"] != null)
                dvgPrecios.Columns["cantLitros"].HeaderText = "Litros";

            if (dvgPrecios.Columns["impGasMes"] != null)
                dvgPrecios.Columns["impGasMes"].HeaderText = "Gas. Mens.";

            if (dvgPrecios.Columns["diasFalta"] != null)
                dvgPrecios.Columns["diasFalta"].HeaderText = "Dias Falta";

            if (dvgPrecios.Columns["impFalta"] != null)
                dvgPrecios.Columns["impFalta"].HeaderText = "Imp. Faltas";

            if (dvgPrecios.Columns["diasIncap"] != null)
                dvgPrecios.Columns["diasIncap"].HeaderText = "Dias Incap.";

            if (dvgPrecios.Columns["impIncap"] != null)
                dvgPrecios.Columns["impIncap"].HeaderText = "Imp. Incap.";

            if (dvgPrecios.Columns["totalDias"] != null)
                dvgPrecios.Columns["totalDias"].HeaderText = "Dias Asign.";

            if (dvgPrecios.Columns["totalMes"] != null)
                dvgPrecios.Columns["totalMes"].HeaderText = "Total";

            if (dvgPrecios.Columns["netoMes"] != null)
                dvgPrecios.Columns["netoMes"].HeaderText = "Neto";

            if (dvgPrecios.Columns["tarjeta"] != null)
                dvgPrecios.Columns["tarjeta"].HeaderText = "Tarjeta";

            dvgPrecios.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dvgPrecios.MultiSelect = false;

            dvgPrecios.AllowUserToAddRows = false;

            if (dvgPrecios.Columns["netoMes"] != null)
            {
                dvgPrecios.Columns["netoMes"]
                    .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }

            if (dvgPrecios.Columns["totalMes"] != null)
            {
                dvgPrecios.Columns["totalMes"]
                    .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight;
            }

            dvgPrecios.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            dvgPrecios.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            dvgPrecios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dvgPrecios.EnableHeadersVisualStyles =      false;
        }

        private void CargarMeses()
        {
            cmbMes.Items.Clear();

            cmbMes.Items.Add("Enero");
            cmbMes.Items.Add("Febrero");
            cmbMes.Items.Add("Marzo");
            cmbMes.Items.Add("Abril");
            cmbMes.Items.Add("Mayo");
            cmbMes.Items.Add("Junio");
            cmbMes.Items.Add("Julio");
            cmbMes.Items.Add("Agosto");
            cmbMes.Items.Add("Septiembre");
            cmbMes.Items.Add("Octubre");
            cmbMes.Items.Add("Noviembre");
            cmbMes.Items.Add("Diciembre");
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult resultado =
                MessageBox.Show(
                    "¿Desea salir del sistema?",
                    "Reporte de Gasolina",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dvgPrecios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                      
        }
        private void CargarPeriodoActual()
        {
            try
            {
                SqlHelper sql =
                    new SqlHelper(
                        AppSettings.ConnectionString);

                object anio =
                    sql.ExecuteScalar(
                        @"SELECT MAX(anio)
                  FROM Ls_HistPrecioGasolinaTbl
                  WHERE compania = @compania",
                        CommandType.Text,
                        new SqlParameter(
                            "@compania",
                            AppSettings.Compania));

                if (anio == null ||
                    anio == DBNull.Value)
                {
                    MessageBox.Show(
                        "No existen periodos configurados.",
                        "Reporte Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                nudAnio.Text = anio.ToString();

                object mes =
                    sql.ExecuteScalar(
                        @"SELECT MAX(mes)
                  FROM Ls_HistPrecioGasolinaTbl
                  WHERE compania = @compania
                  AND anio = @anio",
                        CommandType.Text,
                        new SqlParameter(
                            "@compania",
                            AppSettings.Compania),

                        new SqlParameter(
                            "@anio",
                            Convert.ToInt32(anio)));

                if (mes == null ||
                    mes == DBNull.Value)
                {
                    return;
                }

                int numeroMes =
                    Convert.ToInt32(mes);

                cmbMes.SelectedIndex =
                    numeroMes - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error al obtener período",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void ConsultarPrecios()
        {
            SqlHelper sql =
                new SqlHelper(
                    AppSettings.ConnectionString);

            DataTable dt =
                sql.Execute(
                    @"SELECT
                ciudad,
                precio
              FROM Ls_HistPrecioGasolinaTbl
              WHERE compania = @compania
              AND anio = @anio
              AND mes = @mes
              ORDER BY ciudad",
                    CommandType.Text,

                    new SqlParameter(
                        "@compania",
                        AppSettings.Compania),

                    new SqlParameter(
                        "@anio",
                        Convert.ToInt32(nudAnio.Text)),

                    new SqlParameter(
                        "@mes",
                        cmbMes.SelectedIndex + 1));

            dvgPrecios.DataSource = dt;

            // Encabezados
            dvgPrecios.Columns["ciudad"].HeaderText = "Ciudad";
            dvgPrecios.Columns["precio"].HeaderText = "Precio";

            // Alineaciones
            dvgPrecios.Columns["ciudad"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dvgPrecios.Columns["precio"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            // Formato monetario
            dvgPrecios.Columns["precio"].DefaultCellStyle.Format =
                "N2";

            // Quitar apariencia de enlace/subrayado
            dvgPrecios.DefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular);

            dvgPrecios.RowsDefaultCellStyle.Font =
                new Font("Segoe UI", 9F, FontStyle.Regular);

            // Ajustes visuales
            dvgPrecios.Columns["ciudad"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;

            dvgPrecios.Columns["precio"].Width = 80;

            dvgPrecios.RowHeadersVisible = false;

            if (dvgPrecios.Columns["Region"] != null)
                dvgPrecios.Columns["Region"].HeaderText = "Región";

            if (dvgPrecios.Columns["DepZona"] != null)
                dvgPrecios.Columns["DepZona"].HeaderText = "Dep-Zona";

            if (dvgPrecios.Columns["trabajador"] != null)
                dvgPrecios.Columns["trabajador"].HeaderText = "Trabajador";

            if (dvgPrecios.Columns["nombre"] != null)
                dvgPrecios.Columns["nombre"].HeaderText = "Nombre";

            if (dvgPrecios.Columns["nss"] != null)
                dvgPrecios.Columns["nss"].HeaderText = "NSS";

            if (dvgPrecios.Columns["Ciudad"] != null)
                dvgPrecios.Columns["Ciudad"].HeaderText = "Ciudad";

            if (dvgPrecios.Columns["pvpLitro"] != null)
                dvgPrecios.Columns["pvpLitro"].HeaderText = "PVP Ciudad";

            if (dvgPrecios.Columns["cantLitros"] != null)
                dvgPrecios.Columns["cantLitros"].HeaderText = "Litros";

            if (dvgPrecios.Columns["impGasMes"] != null)
                dvgPrecios.Columns["impGasMes"].HeaderText = "Gas. Mens.";

            if (dvgPrecios.Columns["diasFalta"] != null)
                dvgPrecios.Columns["diasFalta"].HeaderText = "Dias Falta";

            if (dvgPrecios.Columns["impFalta"] != null)
                dvgPrecios.Columns["impFalta"].HeaderText = "Imp. Faltas";

            if (dvgPrecios.Columns["diasIncap"] != null)
                dvgPrecios.Columns["diasIncap"].HeaderText = "Dias Incap.";

            if (dvgPrecios.Columns["impIncap"] != null)
                dvgPrecios.Columns["impIncap"].HeaderText = "Imp. Incap.";

            if (dvgPrecios.Columns["totalDias"] != null)
                dvgPrecios.Columns["totalDias"].HeaderText = "Dias Asign.";

            if (dvgPrecios.Columns["totalMes"] != null)
                dvgPrecios.Columns["totalMes"].HeaderText = "Total";

            if (dvgPrecios.Columns["netoMes"] != null)
                dvgPrecios.Columns["netoMes"].HeaderText = "Neto";

            if (dvgPrecios.Columns["tarjeta"] != null)
                dvgPrecios.Columns["tarjeta"].HeaderText = "Tarjeta";

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultarPrecios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnImportarExcel_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                OpenFileDialog ofd =
                    new OpenFileDialog();

                ofd.Filter =
                    "Archivos Excel (*.xlsx)|*.xlsx";

                ofd.Title =
                    "Seleccione archivo Excel";

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                CargaPrecioGasolinaResult carga = _excelService.LeerArchivo(ofd.FileName);

                SpResult depuracion = _gasolinaService.DepurarPeriodo(
                             AppSettings.Compania,
                                         carga.Anio,
                                         carga.Mes,
                                         AppSettings.Usuario,
                                         AppSettings.Operacion);

                if (depuracion.IdError > 0)
                {
                    MessageBox.Show(
                        depuracion.MensajeError,
                        "Depuración",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                bool existenErrores = false;

                foreach (PrecioGasolinaModel item
                         in carga.Registros)
                {
                    SpResult resultadoValidacion =
                        _gasolinaService.ValidarPrecio(
                            AppSettings.Compania,
                            carga.Anio,
                            carga.Mes,
                            item.Ciudad,
                            item.Precio,
                            AppSettings.Usuario,
                            AppSettings.Operacion);

                    item.Mensaje =
                        resultadoValidacion.MensajeError;

                    if (resultadoValidacion.IdError > 0)
                    {
                        existenErrores = true;
                    }
                }

                if (existenErrores)
                {
                    cmbMes.SelectedIndex =
                        carga.Mes - 1;

                    nudAnio.Text =
                        carga.Anio.ToString();

                    dvgPrecios.AutoGenerateColumns =
                        true;

                    dvgPrecios.DataSource = null;

                    dvgPrecios.DataSource =
                        carga.Registros;

                    if (dvgPrecios.Columns["FilaExcel"] != null)
                    {
                        dvgPrecios.Columns["FilaExcel"].Visible = false;
                    }

                    if (dvgPrecios.Columns["EsValido"] != null)
                    {
                        dvgPrecios.Columns["EsValido"].Visible = false;
                    }

                    MessageBox.Show(
                        "El archivo contiene errores. Revise el detalle mostrado.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int registrosGuardados = 0;

                foreach (PrecioGasolinaModel item
                         in carga.Registros)
                {
                    SpResult resultadoAlta =
                        _gasolinaService.GuardarPrecio(
                            AppSettings.Compania,
                            carga.Anio,
                            carga.Mes,
                            item.Ciudad,
                            item.Precio,
                            AppSettings.Usuario,
                            AppSettings.Operacion);

                    if (resultadoAlta.IdError > 0)
                    {
                        MessageBox.Show(
                            resultadoAlta.MensajeError,
                            "Error al guardar",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return;
                    }

                    registrosGuardados++;
                }

                ConsultarPrecios();

                MessageBox.Show(
                    string.Format(
                        "Proceso terminado correctamente.\r\n\r\nRegistros cargados: {0}",
                        registrosGuardados),
                    "Carga de Precios de Gasolina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExcel_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string directorioSalida =
                    _reporteService.ObtenerDirectorioSalida();

                if (string.IsNullOrWhiteSpace(
                        directorioSalida))
                {
                    MessageBox.Show(
                        "No existe configuración del directorio de salida (dirsalgas).",
                        "Reporte Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!System.IO.Directory.Exists(
                        directorioSalida))
                {
                    System.IO.Directory.CreateDirectory(
                        directorioSalida);
                }

                string archivo =
                    System.IO.Path.Combine(
                        directorioSalida,
                        $"Precio Gasolina por Zona {cmbMes.SelectedIndex + 1:00}-{nudAnio.Text}.xlsx");

                _excelExportService
                    .ExportarPreciosGasolina(
                        archivo,
                        dvgPrecios,
                        cmbMes.SelectedIndex + 1,
                        Convert.ToInt32(
                            nudAnio.Text),
                        txtOperacion.Text,
                        txtUsuario.Text);

                MessageBox.Show(
                    $"Exportación realizada correctamente.\r\n\r\n" +
                    $"Archivo:\r\n{archivo}",
                    "Reporte Gasolina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (System.IO.File.Exists(
                        archivo))
                {
                    Process.Start(
                        new ProcessStartInfo()
                        {
                            FileName = archivo,
                            UseShellExecute = true
                        });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnProcesar_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbMes.SelectedIndex < 0)
                {
                    MessageBox.Show(
                        "Seleccione un mes.",
                        "Reporte Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (string.IsNullOrWhiteSpace(nudAnio.Text))
                {
                    MessageBox.Show(
                        "Seleccione un año.",
                        "Reporte Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                btnConsultar.Enabled = false;
                btnImportarExcel.Enabled = false;
                btnProcesar.Enabled = false;
                btnExcel.Enabled = false;

                Cursor = Cursors.WaitCursor;

                int anio = Convert.ToInt32(nudAnio.Text);

                int mes = cmbMes.SelectedIndex + 1;

                string directorioSalida =
                    _reporteService.ObtenerDirectorioSalida();

                if (string.IsNullOrWhiteSpace(directorioSalida))
                {
                    MessageBox.Show(
                        "No existe configuración del directorio de salida (dirsalgas).",
                        "Reporte Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (!System.IO.Directory.Exists(directorioSalida))
                {
                    System.IO.Directory.CreateDirectory(
                        directorioSalida);
                }

                string archivo =
                    System.IO.Path.Combine(
                        directorioSalida,
                        $"AsignacionGasolina {mes:00}-{anio}.xlsx");

                SpResult resultado =
                    _reporteService.ProcesarReporte(
                        AppSettings.Compania,
                        anio,
                        mes,
                        AppSettings.Usuario,
                        AppSettings.Operacion);

                if (resultado.IdError > 0)
                {
                    MessageBox.Show(
                        resultado.MensajeError,
                        "Proceso de Gasolina",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                DataTable dt =
                    _reporteService.ObtenerReporte(
                        AppSettings.Compania,
                        anio,
                        mes);

                dvgPrecios.AutoGenerateColumns = true;
                dvgPrecios.DataSource = null;
                dvgPrecios.DataSource = dt;

                FormatearGridReporte();

                DataTable dtAltas = _reporteService.ObtenerAltas(
                        AppSettings.Compania,
                        anio,
                        mes);

               DataTable dtFaltas = _reporteService.ObtenerFaltas(
                     AppSettings.Compania,
                                 anio,
                                 mes);

//
                DataTable dtIncapacidades = _reporteService.ObtenerIncapacidades(
                     AppSettings.Compania,
                                 anio,
                                 mes);
                _excelReporteGasolinaService
                    .ExportarReporteCompleto(
                        archivo,
                        dt,
                        dtAltas,
                        dtFaltas,
                        dtIncapacidades,
                        mes,
                        anio,
                        txtOperacion.Text,
                        txtUsuario.Text);

//
                MessageBox.Show(
                    "Proceso de gasolina concluido correctamente.\r\n\r\n" +
                    $"Registros obtenidos: {dt.Rows.Count}\r\n\r\n" +
                    $"Archivo generado:\r\n{archivo}",
                    "Reporte Gasolina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (System.IO.File.Exists(archivo))
                {
                    Process.Start(
                        new ProcessStartInfo()
                        {
                            FileName = archivo,
                            UseShellExecute = true
                        });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;

                btnConsultar.Enabled = true;
                btnImportarExcel.Enabled = true;
                btnProcesar.Enabled = true;
                btnExcel.Enabled = true;
            }
        }
    }

}

