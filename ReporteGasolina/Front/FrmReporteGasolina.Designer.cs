namespace ReporteGasolina
{
    partial class FrmReporteGasolina
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnImportarExcel = new System.Windows.Forms.Button();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.LblOperacion = new System.Windows.Forms.Label();
            this.txtFechaProceso = new System.Windows.Forms.TextBox();
            this.LblFechaProceso = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.grpParametros = new System.Windows.Forms.GroupBox();
            this.lblAnio = new System.Windows.Forms.Label();
            this.cmbMes = new System.Windows.Forms.ComboBox();
            this.LblMes = new System.Windows.Forms.Label();
            this.grpCostoGasolina = new System.Windows.Forms.GroupBox();
            this.dvgPrecios = new System.Windows.Forms.DataGridView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.nudAnio = new System.Windows.Forms.NumericUpDown();
            this.pnlHeader.SuspendLayout();
            this.grpParametros.SuspendLayout();
            this.grpCostoGasolina.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgPrecios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnio)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnSalir);
            this.pnlHeader.Controls.Add(this.btnProcesar);
            this.pnlHeader.Controls.Add(this.btnExcel);
            this.pnlHeader.Controls.Add(this.btnImportarExcel);
            this.pnlHeader.Controls.Add(this.btnConsultar);
            this.pnlHeader.Controls.Add(this.txtUsuario);
            this.pnlHeader.Controls.Add(this.lblUsuario);
            this.pnlHeader.Controls.Add(this.txtOperacion);
            this.pnlHeader.Controls.Add(this.LblOperacion);
            this.pnlHeader.Controls.Add(this.txtFechaProceso);
            this.pnlHeader.Controls.Add(this.LblFechaProceso);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 109);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // btnSalir
            // 
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Image = global::ReporteGasolina.Properties.Resources.salir;
            this.btnSalir.Location = new System.Drawing.Point(184, 0);
            this.btnSalir.Margin = new System.Windows.Forms.Padding(2);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(43, 42);
            this.btnSalir.TabIndex = 11;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcesar.Image = global::ReporteGasolina.Properties.Resources.procesar;
            this.btnProcesar.Location = new System.Drawing.Point(137, 0);
            this.btnProcesar.Margin = new System.Windows.Forms.Padding(2);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(43, 42);
            this.btnProcesar.TabIndex = 10;
            this.btnProcesar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.Image = global::ReporteGasolina.Properties.Resources.Excel;
            this.btnExcel.Location = new System.Drawing.Point(91, 0);
            this.btnExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(43, 42);
            this.btnExcel.TabIndex = 9;
            this.btnExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnImportarExcel
            // 
            this.btnImportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportarExcel.Image = global::ReporteGasolina.Properties.Resources.importar;
            this.btnImportarExcel.Location = new System.Drawing.Point(45, 0);
            this.btnImportarExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btnImportarExcel.Name = "btnImportarExcel";
            this.btnImportarExcel.Size = new System.Drawing.Size(43, 42);
            this.btnImportarExcel.TabIndex = 8;
            this.btnImportarExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnImportarExcel.UseVisualStyleBackColor = true;
            this.btnImportarExcel.Click += new System.EventHandler(this.btnImportarExcel_Click);
            // 
            // btnConsultar
            // 
            this.btnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultar.Image = global::ReporteGasolina.Properties.Resources.consultar;
            this.btnConsultar.Location = new System.Drawing.Point(0, 0);
            this.btnConsultar.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(43, 42);
            this.btnConsultar.TabIndex = 7;
            this.btnConsultar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnConsultar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(654, 59);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(126, 13);
            this.txtUsuario.TabIndex = 6;
            // 
            // lblUsuario
            // 
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(581, 59);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(67, 18);
            this.lblUsuario.TabIndex = 5;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtOperacion
            // 
            this.txtOperacion.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtOperacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperacion.Location = new System.Drawing.Point(654, 33);
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.ReadOnly = true;
            this.txtOperacion.Size = new System.Drawing.Size(126, 13);
            this.txtOperacion.TabIndex = 4;
            // 
            // LblOperacion
            // 
            this.LblOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblOperacion.Location = new System.Drawing.Point(581, 33);
            this.LblOperacion.Name = "LblOperacion";
            this.LblOperacion.Size = new System.Drawing.Size(82, 18);
            this.LblOperacion.TabIndex = 3;
            this.LblOperacion.Text = "Operación:";
            // 
            // txtFechaProceso
            // 
            this.txtFechaProceso.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtFechaProceso.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFechaProceso.Enabled = false;
            this.txtFechaProceso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaProceso.Location = new System.Drawing.Point(654, 6);
            this.txtFechaProceso.Name = "txtFechaProceso";
            this.txtFechaProceso.ReadOnly = true;
            this.txtFechaProceso.Size = new System.Drawing.Size(143, 13);
            this.txtFechaProceso.TabIndex = 2;
            // 
            // LblFechaProceso
            // 
            this.LblFechaProceso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFechaProceso.Location = new System.Drawing.Point(581, 6);
            this.LblFechaProceso.Name = "LblFechaProceso";
            this.LblFechaProceso.Size = new System.Drawing.Size(79, 18);
            this.LblFechaProceso.TabIndex = 1;
            this.LblFechaProceso.Text = "Fecha:";
            // 
            // lblTitulo
            // 
            this.lblTitulo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(3, 72);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(797, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "REPORTE DE GASOLINA";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // grpParametros
            // 
            this.grpParametros.Controls.Add(this.nudAnio);
            this.grpParametros.Controls.Add(this.lblAnio);
            this.grpParametros.Controls.Add(this.cmbMes);
            this.grpParametros.Controls.Add(this.LblMes);
            this.grpParametros.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpParametros.Location = new System.Drawing.Point(0, 115);
            this.grpParametros.Name = "grpParametros";
            this.grpParametros.Size = new System.Drawing.Size(800, 57);
            this.grpParametros.TabIndex = 1;
            this.grpParametros.TabStop = false;
            this.grpParametros.Text = "Parámetros de Proceso:";
            // 
            // lblAnio
            // 
            this.lblAnio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnio.Location = new System.Drawing.Point(343, 25);
            this.lblAnio.Name = "lblAnio";
            this.lblAnio.Size = new System.Drawing.Size(43, 18);
            this.lblAnio.TabIndex = 4;
            this.lblAnio.Text = "Año:";
            // 
            // cmbMes
            // 
            this.cmbMes.FormattingEnabled = true;
            this.cmbMes.Location = new System.Drawing.Point(55, 25);
            this.cmbMes.Name = "cmbMes";
            this.cmbMes.Size = new System.Drawing.Size(187, 23);
            this.cmbMes.TabIndex = 3;
            // 
            // LblMes
            // 
            this.LblMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMes.Location = new System.Drawing.Point(6, 25);
            this.LblMes.Name = "LblMes";
            this.LblMes.Size = new System.Drawing.Size(43, 18);
            this.LblMes.TabIndex = 2;
            this.LblMes.Text = "Mes:";
            // 
            // grpCostoGasolina
            // 
            this.grpCostoGasolina.Controls.Add(this.dvgPrecios);
            this.grpCostoGasolina.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCostoGasolina.Location = new System.Drawing.Point(0, 180);
            this.grpCostoGasolina.Name = "grpCostoGasolina";
            this.grpCostoGasolina.Size = new System.Drawing.Size(800, 407);
            this.grpCostoGasolina.TabIndex = 2;
            this.grpCostoGasolina.TabStop = false;
            this.grpCostoGasolina.Text = "Costo Gasolina por Zona";
            // 
            // dvgPrecios
            // 
            this.dvgPrecios.AllowUserToAddRows = false;
            this.dvgPrecios.AllowUserToDeleteRows = false;
            this.dvgPrecios.AllowUserToResizeRows = false;
            this.dvgPrecios.BackgroundColor = System.Drawing.Color.White;
            this.dvgPrecios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dvgPrecios.DefaultCellStyle = dataGridViewCellStyle1;
            this.dvgPrecios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvgPrecios.Location = new System.Drawing.Point(3, 19);
            this.dvgPrecios.Margin = new System.Windows.Forms.Padding(2);
            this.dvgPrecios.MultiSelect = false;
            this.dvgPrecios.Name = "dvgPrecios";
            this.dvgPrecios.ReadOnly = true;
            this.dvgPrecios.RowTemplate.Height = 28;
            this.dvgPrecios.Size = new System.Drawing.Size(794, 385);
            this.dvgPrecios.TabIndex = 0;
            this.dvgPrecios.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgPrecios_CellContentClick);
            // 
            // nudAnio
            // 
            this.nudAnio.Location = new System.Drawing.Point(392, 25);
            this.nudAnio.Maximum = new decimal(new int[] {
            2040,
            0,
            0,
            0});
            this.nudAnio.Minimum = new decimal(new int[] {
            2013,
            0,
            0,
            0});
            this.nudAnio.Name = "nudAnio";
            this.nudAnio.Size = new System.Drawing.Size(65, 23);
            this.nudAnio.TabIndex = 6;
            this.nudAnio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAnio.Value = new decimal(new int[] {
            2013,
            0,
            0,
            0});
            // 
            // FrmReporteGasolina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 599);
            this.ControlBox = false;
            this.Controls.Add(this.grpCostoGasolina);
            this.Controls.Add(this.grpParametros);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmReporteGasolina";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.grpParametros.ResumeLayout(false);
            this.grpCostoGasolina.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvgPrecios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAnio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label LblFechaProceso;
        private System.Windows.Forms.TextBox txtOperacion;
        private System.Windows.Forms.Label LblOperacion;
        private System.Windows.Forms.TextBox txtFechaProceso;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.GroupBox grpParametros;
        private System.Windows.Forms.Label LblMes;
        private System.Windows.Forms.Label lblAnio;
        private System.Windows.Forms.ComboBox cmbMes;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnImportarExcel;
        private System.Windows.Forms.GroupBox grpCostoGasolina;
        private System.Windows.Forms.DataGridView dvgPrecios;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nudAnio;
    }
}