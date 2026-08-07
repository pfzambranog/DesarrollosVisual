using ReporteGasolina.Infrastructure;
using ReporteGasolina.Models;
using ReporteGasolina.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

                cmbAnio.Text = anio.ToString();

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
                        Convert.ToInt32(cmbAnio.Text)),

                    new SqlParameter(
                        "@mes",
                        cmbMes.SelectedIndex + 1));

            dvgPrecios.DataSource = dt;


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

                CargaPrecioGasolinaResult carga =
                    _excelService.LeerArchivo(
                        ofd.FileName);

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

                    cmbAnio.Text =
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd =
                    new SaveFileDialog();

                sfd.Filter =
                    "Excel (*.xlsx)|*.xlsx";

                sfd.FileName =
                    $"Precio Gasolina por Zona {cmbMes.SelectedIndex + 1:00}-{cmbAnio.Text}.xlsx";

                if (sfd.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }

                _excelExportService
                    .ExportarPreciosGasolina(
                        sfd.FileName,
                        dvgPrecios,
                        cmbMes.SelectedIndex + 1,
                        Convert.ToInt32(cmbAnio.Text),
                        txtOperacion.Text,
                        txtUsuario.Text);

                MessageBox.Show(
                    "Exportación a Excel realizada correctamente.",
                    "Reporte Gasolina",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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
    }

}

