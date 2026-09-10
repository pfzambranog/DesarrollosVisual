namespace SCMBD
{
    partial class FrmManMenus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManMenus));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbEstatus = new System.Windows.Forms.ComboBox();
            this.LblEstatus = new System.Windows.Forms.Label();
            this.TxtOrdenPresentacion = new System.Windows.Forms.TextBox();
            this.LblOrdenPresentacion = new System.Windows.Forms.Label();
            this.TxtDescripcion = new System.Windows.Forms.TextBox();
            this.LblMenu = new System.Windows.Forms.Label();
            this.TxtClaveMenu = new System.Windows.Forms.TextBox();
            this.LblClaveMenu = new System.Windows.Forms.Label();
            this.TxtIdMenu = new System.Windows.Forms.TextBox();
            this.LblIdMenu = new System.Windows.Forms.Label();
            this.pnlBarrainicial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.pnlBarrainicial.Size = new System.Drawing.Size(1256, 92);
            this.pnlBarrainicial.TabIndex = 2;
            this.pnlBarrainicial.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBarrainicial_Paint);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(1001, 2);
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
            this.BtnExcel.Location = new System.Drawing.Point(1060, 2);
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
            this.BtnRefrescar.Location = new System.Drawing.Point(1115, 2);
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
            this.BtnSalir.Location = new System.Drawing.Point(1174, 2);
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
            this.txtUsuario.Location = new System.Drawing.Point(1155, 69);
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
            this.txtOperacion.Location = new System.Drawing.Point(1155, 50);
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
            this.panel1.Controls.Add(this.cmbEstatus);
            this.panel1.Controls.Add(this.LblEstatus);
            this.panel1.Controls.Add(this.TxtOrdenPresentacion);
            this.panel1.Controls.Add(this.LblOrdenPresentacion);
            this.panel1.Controls.Add(this.TxtDescripcion);
            this.panel1.Controls.Add(this.LblMenu);
            this.panel1.Controls.Add(this.TxtClaveMenu);
            this.panel1.Controls.Add(this.LblClaveMenu);
            this.panel1.Controls.Add(this.TxtIdMenu);
            this.panel1.Controls.Add(this.LblIdMenu);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1256, 128);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(134, 91);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(218, 23);
            this.cmbEstatus.TabIndex = 11;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblEstatus.Location = new System.Drawing.Point(4, 91);
            this.LblEstatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEstatus.Name = "LblEstatus";
            this.LblEstatus.Size = new System.Drawing.Size(47, 15);
            this.LblEstatus.TabIndex = 10;
            this.LblEstatus.Text = "Estatus:";
            // 
            // TxtOrdenPresentacion
            // 
            this.TxtOrdenPresentacion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtOrdenPresentacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtOrdenPresentacion.Location = new System.Drawing.Point(566, 57);
            this.TxtOrdenPresentacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtOrdenPresentacion.Name = "TxtOrdenPresentacion";
            this.TxtOrdenPresentacion.Size = new System.Drawing.Size(160, 23);
            this.TxtOrdenPresentacion.TabIndex = 7;
            // 
            // LblOrdenPresentacion
            // 
            this.LblOrdenPresentacion.AutoSize = true;
            this.LblOrdenPresentacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblOrdenPresentacion.Location = new System.Drawing.Point(428, 60);
            this.LblOrdenPresentacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblOrdenPresentacion.Name = "LblOrdenPresentacion";
            this.LblOrdenPresentacion.Size = new System.Drawing.Size(130, 15);
            this.LblOrdenPresentacion.TabIndex = 6;
            this.LblOrdenPresentacion.Text = "Orden de Presentacion:";
            // 
            // TxtDescripcion
            // 
            this.TxtDescripcion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtDescripcion.Location = new System.Drawing.Point(134, 54);
            this.TxtDescripcion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtDescripcion.Name = "TxtDescripcion";
            this.TxtDescripcion.Size = new System.Drawing.Size(279, 23);
            this.TxtDescripcion.TabIndex = 5;
            // 
            // LblMenu
            // 
            this.LblMenu.AutoSize = true;
            this.LblMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblMenu.Location = new System.Drawing.Point(4, 57);
            this.LblMenu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblMenu.Name = "LblMenu";
            this.LblMenu.Size = new System.Drawing.Size(41, 15);
            this.LblMenu.TabIndex = 4;
            this.LblMenu.Text = "Menu:";
            // 
            // TxtClaveMenu
            // 
            this.TxtClaveMenu.BackColor = System.Drawing.SystemColors.Control;
            this.TxtClaveMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtClaveMenu.Location = new System.Drawing.Point(566, 16);
            this.TxtClaveMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtClaveMenu.Name = "TxtClaveMenu";
            this.TxtClaveMenu.Size = new System.Drawing.Size(160, 23);
            this.TxtClaveMenu.TabIndex = 2;
            // 
            // LblClaveMenu
            // 
            this.LblClaveMenu.AutoSize = true;
            this.LblClaveMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblClaveMenu.Location = new System.Drawing.Point(428, 24);
            this.LblClaveMenu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClaveMenu.Name = "LblClaveMenu";
            this.LblClaveMenu.Size = new System.Drawing.Size(83, 15);
            this.LblClaveMenu.TabIndex = 2;
            this.LblClaveMenu.Text = "Código Menú:";
            // 
            // TxtIdMenu
            // 
            this.TxtIdMenu.BackColor = System.Drawing.SystemColors.Control;
            this.TxtIdMenu.Enabled = false;
            this.TxtIdMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtIdMenu.Location = new System.Drawing.Point(134, 13);
            this.TxtIdMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtIdMenu.Name = "TxtIdMenu";
            this.TxtIdMenu.Size = new System.Drawing.Size(101, 23);
            this.TxtIdMenu.TabIndex = 1;
            // 
            // LblIdMenu
            // 
            this.LblIdMenu.AutoSize = true;
            this.LblIdMenu.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblIdMenu.Location = new System.Drawing.Point(4, 16);
            this.LblIdMenu.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblIdMenu.Name = "LblIdMenu";
            this.LblIdMenu.Size = new System.Drawing.Size(51, 15);
            this.LblIdMenu.TabIndex = 0;
            this.LblIdMenu.Text = "Id Menu";
            // 
            // FrmManMenus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1256, 639);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBarrainicial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManMenus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmManMenus_Load);
            this.pnlBarrainicial.ResumeLayout(false);
            this.pnlBarrainicial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox TxtIdMenu;
        private System.Windows.Forms.Label LblIdMenu;
        private System.Windows.Forms.Label LblClaveMenu;
        private System.Windows.Forms.TextBox TxtClaveMenu;
        private System.Windows.Forms.TextBox TxtDescripcion;
        private System.Windows.Forms.Label LblMenu;
        private System.Windows.Forms.TextBox TxtOrdenPresentacion;
        private System.Windows.Forms.Label LblOrdenPresentacion;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox cmbEstatus;
    }
}
