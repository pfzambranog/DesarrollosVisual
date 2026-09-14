namespace SCMBD
{
    partial class FrmRelUsuarioOperacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRelUsuarioOperacion));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblOperaciones = new System.Windows.Forms.Label();
            this.txtFiltroOperaciones = new System.Windows.Forms.TextBox();
            this.cmbAutorizaciones = new System.Windows.Forms.ComboBox();
            this.LblAutorizaciones = new System.Windows.Forms.Label();
            this.CmbUsuarios = new System.Windows.Forms.ComboBox();
            this.LblUsuarios = new System.Windows.Forms.Label();
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
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(601, 2);
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
            this.panel1.Controls.Add(this.LblOperaciones);
            this.panel1.Controls.Add(this.txtFiltroOperaciones);
            this.panel1.Controls.Add(this.cmbAutorizaciones);
            this.panel1.Controls.Add(this.LblAutorizaciones);
            this.panel1.Controls.Add(this.CmbUsuarios);
            this.panel1.Controls.Add(this.LblUsuarios);
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
            // txtFiltroOperaciones
            // 
            this.txtFiltroOperaciones.Location = new System.Drawing.Point(6, 100);
            this.txtFiltroOperaciones.Name = "txtFiltroOperaciones";
            this.txtFiltroOperaciones.Size = new System.Drawing.Size(403, 20);
            this.txtFiltroOperaciones.TabIndex = 16;
            // 
            // cmbAutorizaciones
            // 
            this.cmbAutorizaciones.BackColor = System.Drawing.SystemColors.Control;
            this.cmbAutorizaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbAutorizaciones.FormattingEnabled = true;
            this.cmbAutorizaciones.Location = new System.Drawing.Point(410, 98);
            this.cmbAutorizaciones.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbAutorizaciones.Name = "cmbAutorizaciones";
            this.cmbAutorizaciones.Size = new System.Drawing.Size(210, 23);
            this.cmbAutorizaciones.TabIndex = 15;
            // 
            // LblAutorizaciones
            // 
            this.LblAutorizaciones.AutoSize = true;
            this.LblAutorizaciones.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblAutorizaciones.Location = new System.Drawing.Point(451, 81);
            this.LblAutorizaciones.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblAutorizaciones.Name = "LblAutorizaciones";
            this.LblAutorizaciones.Size = new System.Drawing.Size(88, 15);
            this.LblAutorizaciones.TabIndex = 14;
            this.LblAutorizaciones.Text = "Autorizaciones:";
            // 
            // CmbUsuarios
            // 
            this.CmbUsuarios.BackColor = System.Drawing.SystemColors.Control;
            this.CmbUsuarios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CmbUsuarios.FormattingEnabled = true;
            this.CmbUsuarios.Location = new System.Drawing.Point(254, 26);
            this.CmbUsuarios.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CmbUsuarios.Name = "CmbUsuarios";
            this.CmbUsuarios.Size = new System.Drawing.Size(265, 23);
            this.CmbUsuarios.TabIndex = 13;
            // 
            // LblUsuarios
            // 
            this.LblUsuarios.AutoSize = true;
            this.LblUsuarios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblUsuarios.Location = new System.Drawing.Point(375, 8);
            this.LblUsuarios.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblUsuarios.Name = "LblUsuarios";
            this.LblUsuarios.Size = new System.Drawing.Size(55, 15);
            this.LblUsuarios.TabIndex = 12;
            this.LblUsuarios.Text = "Usuarios:";
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(622, 98);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(210, 23);
            this.cmbEstatus.TabIndex = 11;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblEstatus.Location = new System.Drawing.Point(697, 81);
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
            // FrmRelUsuarioOperacion
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
            this.Name = "FrmRelUsuarioOperacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmRelUsuarioOperacion_Load);
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
        private System.Windows.Forms.TextBox txtFiltroOperaciones;
        private System.Windows.Forms.ComboBox cmbAutorizaciones;
        private System.Windows.Forms.Label LblAutorizaciones;
        private System.Windows.Forms.ComboBox CmbUsuarios;
        private System.Windows.Forms.Label LblUsuarios;
        private System.Windows.Forms.DataGridView dg;
    }
}
