namespace SCMBD
{
    partial class FrmRelMenuOperacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRelMenuOperacion));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnBaja = new System.Windows.Forms.Button();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtPosicion = new System.Windows.Forms.TextBox();
            this.LblPosicion = new System.Windows.Forms.Label();
            this.cmbOperaciones = new System.Windows.Forms.ComboBox();
            this.LblOperaciones = new System.Windows.Forms.Label();
            this.CmbMenu = new System.Windows.Forms.ComboBox();
            this.LblMenu = new System.Windows.Forms.Label();
            this.cmbEstatus = new System.Windows.Forms.ComboBox();
            this.LblEstatus = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.pnlBarrainicial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBarrainicial
            // 
            this.pnlBarrainicial.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlBarrainicial.Controls.Add(this.BtnBaja);
            this.pnlBarrainicial.Controls.Add(this.BtnProcesar);
            this.pnlBarrainicial.Controls.Add(this.BtnExcel);
            this.pnlBarrainicial.Controls.Add(this.BtnRefrescar);
            this.pnlBarrainicial.Controls.Add(this.BtnSalir);
            this.pnlBarrainicial.Controls.Add(this.txtUsuario);
            this.pnlBarrainicial.Controls.Add(this.txtOperacion);
            this.pnlBarrainicial.Controls.Add(this.picLogo);
            this.pnlBarrainicial.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarrainicial.Location = new System.Drawing.Point(0, 0);
            this.pnlBarrainicial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlBarrainicial.Name = "pnlBarrainicial";
            this.pnlBarrainicial.Size = new System.Drawing.Size(841, 92);
            this.pnlBarrainicial.TabIndex = 2;
            this.pnlBarrainicial.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBarrainicial_Paint);
            // 
            // BtnBaja
            // 
            this.BtnBaja.Image = global::SCMBD.Properties.Resources.Elimina;
            this.BtnBaja.Location = new System.Drawing.Point(611, 3);
            this.BtnBaja.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnBaja.Name = "BtnBaja";
            this.BtnBaja.Size = new System.Drawing.Size(59, 42);
            this.BtnBaja.TabIndex = 13;
            this.BtnBaja.UseVisualStyleBackColor = true;
            this.BtnBaja.Click += new System.EventHandler(this.BtnBaja_Click);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(550, 2);
            this.BtnProcesar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnProcesar.Name = "BtnProcesar";
            this.BtnProcesar.Size = new System.Drawing.Size(59, 42);
            this.BtnProcesar.TabIndex = 12;
            this.BtnProcesar.UseVisualStyleBackColor = true;
            this.BtnProcesar.Click += new System.EventHandler(this.BtnProcesar_Click);
            // 
            // BtnExcel
            // 
            this.BtnExcel.Image = global::SCMBD.Properties.Resources.Excel;
            this.BtnExcel.Location = new System.Drawing.Point(660, 2);
            this.BtnExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnExcel.Name = "BtnExcel";
            this.BtnExcel.Size = new System.Drawing.Size(59, 42);
            this.BtnExcel.TabIndex = 11;
            this.BtnExcel.UseVisualStyleBackColor = true;
            this.BtnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // BtnRefrescar
            // 
            this.BtnRefrescar.Image = global::SCMBD.Properties.Resources.renovar;
            this.BtnRefrescar.Location = new System.Drawing.Point(715, 2);
            this.BtnRefrescar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnRefrescar.Name = "BtnRefrescar";
            this.BtnRefrescar.Size = new System.Drawing.Size(59, 42);
            this.BtnRefrescar.TabIndex = 10;
            this.BtnRefrescar.UseVisualStyleBackColor = true;
            this.BtnRefrescar.Click += new System.EventHandler(this.BtnRefrescar_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.Image = global::SCMBD.Properties.Resources.salir;
            this.BtnSalir.Location = new System.Drawing.Point(774, 2);
            this.BtnSalir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(59, 42);
            this.BtnSalir.TabIndex = 9;
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(755, 69);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(78, 13);
            this.txtUsuario.TabIndex = 8;
            // 
            // txtOperacion
            // 
            this.txtOperacion.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtOperacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperacion.Location = new System.Drawing.Point(755, 50);
            this.txtOperacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.ReadOnly = true;
            this.txtOperacion.Size = new System.Drawing.Size(78, 13);
            this.txtOperacion.TabIndex = 7;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.picLogo.Location = new System.Drawing.Point(6, 5);
            this.picLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(114, 58);
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.TxtPosicion);
            this.panel1.Controls.Add(this.LblPosicion);
            this.panel1.Controls.Add(this.cmbOperaciones);
            this.panel1.Controls.Add(this.LblOperaciones);
            this.panel1.Controls.Add(this.CmbMenu);
            this.panel1.Controls.Add(this.LblMenu);
            this.panel1.Controls.Add(this.cmbEstatus);
            this.panel1.Controls.Add(this.LblEstatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(841, 129);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // TxtPosicion
            // 
            this.TxtPosicion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtPosicion.Location = new System.Drawing.Point(710, 98);
            this.TxtPosicion.MaxLength = 3;
            this.TxtPosicion.Name = "TxtPosicion";
            this.TxtPosicion.Size = new System.Drawing.Size(56, 23);
            this.TxtPosicion.TabIndex = 20;
            this.TxtPosicion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblPosicion
            // 
            this.LblPosicion.AutoSize = true;
            this.LblPosicion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblPosicion.Location = new System.Drawing.Point(712, 81);
            this.LblPosicion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPosicion.Name = "LblPosicion";
            this.LblPosicion.Size = new System.Drawing.Size(55, 15);
            this.LblPosicion.TabIndex = 19;
            this.LblPosicion.Text = "Posicion:";
            // 
            // cmbOperaciones
            // 
            this.cmbOperaciones.BackColor = System.Drawing.SystemColors.Control;
            this.cmbOperaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbOperaciones.FormattingEnabled = true;
            this.cmbOperaciones.Location = new System.Drawing.Point(6, 98);
            this.cmbOperaciones.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbOperaciones.Name = "cmbOperaciones";
            this.cmbOperaciones.Size = new System.Drawing.Size(396, 23);
            this.cmbOperaciones.TabIndex = 18;
            // 
            // LblOperaciones
            // 
            this.LblOperaciones.AutoSize = true;
            this.LblOperaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblOperaciones.Location = new System.Drawing.Point(138, 81);
            this.LblOperaciones.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblOperaciones.Name = "LblOperaciones";
            this.LblOperaciones.Size = new System.Drawing.Size(76, 15);
            this.LblOperaciones.TabIndex = 17;
            this.LblOperaciones.Text = "Operaciones:";
            // 
            // CmbMenu
            // 
            this.CmbMenu.BackColor = System.Drawing.SystemColors.Control;
            this.CmbMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CmbMenu.FormattingEnabled = true;
            this.CmbMenu.Location = new System.Drawing.Point(254, 26);
            this.CmbMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CmbMenu.Name = "CmbMenu";
            this.CmbMenu.Size = new System.Drawing.Size(265, 23);
            this.CmbMenu.TabIndex = 13;
            // 
            // LblMenu
            // 
            this.LblMenu.AutoSize = true;
            this.LblMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblMenu.Location = new System.Drawing.Point(375, 8);
            this.LblMenu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblMenu.Name = "LblMenu";
            this.LblMenu.Size = new System.Drawing.Size(41, 15);
            this.LblMenu.TabIndex = 12;
            this.LblMenu.Text = "Menu:";
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(410, 98);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(210, 23);
            this.cmbEstatus.TabIndex = 11;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblEstatus.Location = new System.Drawing.Point(485, 81);
            this.LblEstatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEstatus.Name = "LblEstatus";
            this.LblEstatus.Size = new System.Drawing.Size(47, 15);
            this.LblEstatus.TabIndex = 10;
            this.LblEstatus.Text = "Estatus:";
            // 
            // dg
            // 
            this.dg.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Location = new System.Drawing.Point(4, 227);
            this.dg.MultiSelect = false;
            this.dg.Name = "dg";
            this.dg.RowTemplate.Height = 28;
            this.dg.Size = new System.Drawing.Size(829, 421);
            this.dg.TabIndex = 4;
            // 
            // FrmRelMenuOperacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(841, 639);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBarrainicial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRelMenuOperacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmRelMenuOperacion_Load);
            this.pnlBarrainicial.ResumeLayout(false);
            this.pnlBarrainicial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBarrainicial;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtOperacion;
        private System.Windows.Forms.Button BtnRefrescar;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.Button BtnExcel;
        private System.Windows.Forms.Button BtnProcesar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox cmbEstatus;
        private System.Windows.Forms.Label LblOperaciones;
        private System.Windows.Forms.ComboBox CmbMenu;
        private System.Windows.Forms.Label LblMenu;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Label LblPosicion;
        private System.Windows.Forms.ComboBox cmbOperaciones;
        private System.Windows.Forms.TextBox TxtPosicion;
        private System.Windows.Forms.Button BtnBaja;
    }
}
