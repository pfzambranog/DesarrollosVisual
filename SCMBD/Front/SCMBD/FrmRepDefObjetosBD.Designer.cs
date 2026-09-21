namespace SCMBD
{
    partial class FrmRepDefObjetosBD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRepDefObjetosBD));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnVerScript = new System.Windows.Forms.Button();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbReportes = new System.Windows.Forms.ComboBox();
            this.LblReportes = new System.Windows.Forms.Label();
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
            this.pnlBarrainicial.Controls.Add(this.BtnVerScript);
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
            this.pnlBarrainicial.Size = new System.Drawing.Size(1540, 92);
            this.pnlBarrainicial.TabIndex = 2;
            this.pnlBarrainicial.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBarrainicial_Paint);
            // 
            // BtnVerScript
            // 
            this.BtnVerScript.Image = global::SCMBD.Properties.Resources.Archivo_Sql2;
            this.BtnVerScript.Location = new System.Drawing.Point(1313, 2);
            this.BtnVerScript.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnVerScript.Name = "BtnVerScript";
            this.BtnVerScript.Size = new System.Drawing.Size(59, 42);
            this.BtnVerScript.TabIndex = 13;
            this.BtnVerScript.UseVisualStyleBackColor = true;
            this.BtnVerScript.Click += new System.EventHandler(this.BtnVerScript_Click);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(1197, 2);
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
            this.BtnExcel.Location = new System.Drawing.Point(1256, 2);
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
            this.BtnRefrescar.Location = new System.Drawing.Point(1371, 2);
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
            this.BtnSalir.Location = new System.Drawing.Point(1430, 2);
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
            this.txtUsuario.Location = new System.Drawing.Point(1410, 69);
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
            this.txtOperacion.Location = new System.Drawing.Point(1410, 50);
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
            this.panel1.Controls.Add(this.CmbReportes);
            this.panel1.Controls.Add(this.LblReportes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1540, 58);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // CmbReportes
            // 
            this.CmbReportes.BackColor = System.Drawing.SystemColors.Control;
            this.CmbReportes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CmbReportes.FormattingEnabled = true;
            this.CmbReportes.Location = new System.Drawing.Point(331, 26);
            this.CmbReportes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CmbReportes.Name = "CmbReportes";
            this.CmbReportes.Size = new System.Drawing.Size(265, 23);
            this.CmbReportes.TabIndex = 13;
            // 
            // LblReportes
            // 
            this.LblReportes.AutoSize = true;
            this.LblReportes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblReportes.Location = new System.Drawing.Point(452, 8);
            this.LblReportes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblReportes.Name = "LblReportes";
            this.LblReportes.Size = new System.Drawing.Size(56, 15);
            this.LblReportes.TabIndex = 12;
            this.LblReportes.Text = "Reportes:";
            // 
            // dg
            // 
            this.dg.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Location = new System.Drawing.Point(4, 156);
            this.dg.MultiSelect = false;
            this.dg.Name = "dg";
            this.dg.RowTemplate.Height = 28;
            this.dg.Size = new System.Drawing.Size(1504, 674);
            this.dg.TabIndex = 4;
            // 
            // FrmRepDefObjetosBD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1540, 825);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBarrainicial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRepDefObjetosBD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmRepDefObjetosBD_Load);
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
        private System.Windows.Forms.ComboBox CmbReportes;
        private System.Windows.Forms.Label LblReportes;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Button BtnVerScript;
    }
}
