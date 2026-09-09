namespace SCMBD
{
    partial class FrmManOperaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManOperaciones));
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
            this.TxtRuta = new System.Windows.Forms.TextBox();
            this.LblRuta = new System.Windows.Forms.Label();
            this.TxtLlamada = new System.Windows.Forms.TextBox();
            this.LblLamada = new System.Windows.Forms.Label();
            this.TxtDescripcion = new System.Windows.Forms.TextBox();
            this.LblOperacion = new System.Windows.Forms.Label();
            this.TxtClaveOperacion = new System.Windows.Forms.TextBox();
            this.LblClaveOperacion = new System.Windows.Forms.Label();
            this.TxtIdOperacion = new System.Windows.Forms.TextBox();
            this.LblIdOperacion = new System.Windows.Forms.Label();
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
            this.panel1.Controls.Add(this.TxtRuta);
            this.panel1.Controls.Add(this.LblRuta);
            this.panel1.Controls.Add(this.TxtLlamada);
            this.panel1.Controls.Add(this.LblLamada);
            this.panel1.Controls.Add(this.TxtDescripcion);
            this.panel1.Controls.Add(this.LblOperacion);
            this.panel1.Controls.Add(this.TxtClaveOperacion);
            this.panel1.Controls.Add(this.LblClaveOperacion);
            this.panel1.Controls.Add(this.TxtIdOperacion);
            this.panel1.Controls.Add(this.LblIdOperacion);
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
            // TxtRuta
            // 
            this.TxtRuta.BackColor = System.Drawing.SystemColors.Control;
            this.TxtRuta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtRuta.Location = new System.Drawing.Point(964, 57);
            this.TxtRuta.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtRuta.Name = "TxtRuta";
            this.TxtRuta.Size = new System.Drawing.Size(289, 23);
            this.TxtRuta.TabIndex = 9;
            // 
            // LblRuta
            // 
            this.LblRuta.AutoSize = true;
            this.LblRuta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblRuta.Location = new System.Drawing.Point(835, 60);
            this.LblRuta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblRuta.Name = "LblRuta";
            this.LblRuta.Size = new System.Drawing.Size(110, 15);
            this.LblRuta.TabIndex = 8;
            this.LblRuta.Text = "Ruta del Ejecutable:";
            // 
            // TxtLlamada
            // 
            this.TxtLlamada.BackColor = System.Drawing.SystemColors.Control;
            this.TxtLlamada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtLlamada.Location = new System.Drawing.Point(542, 57);
            this.TxtLlamada.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtLlamada.Name = "TxtLlamada";
            this.TxtLlamada.Size = new System.Drawing.Size(279, 23);
            this.TxtLlamada.TabIndex = 7;
            // 
            // LblLamada
            // 
            this.LblLamada.AutoSize = true;
            this.LblLamada.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblLamada.Location = new System.Drawing.Point(428, 60);
            this.LblLamada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblLamada.Name = "LblLamada";
            this.LblLamada.Size = new System.Drawing.Size(64, 15);
            this.LblLamada.TabIndex = 6;
            this.LblLamada.Text = "Ejecutable:";
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
            // LblOperacion
            // 
            this.LblOperacion.AutoSize = true;
            this.LblOperacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblOperacion.Location = new System.Drawing.Point(4, 57);
            this.LblOperacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblOperacion.Name = "LblOperacion";
            this.LblOperacion.Size = new System.Drawing.Size(65, 15);
            this.LblOperacion.TabIndex = 4;
            this.LblOperacion.Text = "Operacion:";
            // 
            // TxtClaveOperacion
            // 
            this.TxtClaveOperacion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtClaveOperacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtClaveOperacion.Location = new System.Drawing.Point(542, 16);
            this.TxtClaveOperacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtClaveOperacion.Name = "TxtClaveOperacion";
            this.TxtClaveOperacion.Size = new System.Drawing.Size(160, 23);
            this.TxtClaveOperacion.TabIndex = 2;
            // 
            // LblClaveOperacion
            // 
            this.LblClaveOperacion.AutoSize = true;
            this.LblClaveOperacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblClaveOperacion.Location = new System.Drawing.Point(428, 24);
            this.LblClaveOperacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClaveOperacion.Name = "LblClaveOperacion";
            this.LblClaveOperacion.Size = new System.Drawing.Size(97, 15);
            this.LblClaveOperacion.TabIndex = 2;
            this.LblClaveOperacion.Text = "Clave Operación:";
            // 
            // TxtIdOperacion
            // 
            this.TxtIdOperacion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtIdOperacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtIdOperacion.Location = new System.Drawing.Point(134, 13);
            this.TxtIdOperacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtIdOperacion.Name = "TxtIdOperacion";
            this.TxtIdOperacion.Size = new System.Drawing.Size(101, 23);
            this.TxtIdOperacion.TabIndex = 1;
            // 
            // LblIdOperacion
            // 
            this.LblIdOperacion.AutoSize = true;
            this.LblIdOperacion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblIdOperacion.Location = new System.Drawing.Point(4, 16);
            this.LblIdOperacion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblIdOperacion.Name = "LblIdOperacion";
            this.LblIdOperacion.Size = new System.Drawing.Size(78, 15);
            this.LblIdOperacion.TabIndex = 0;
            this.LblIdOperacion.Text = "Id Operacion:";
            // 
            // FrmManOperaciones
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
            this.Name = "FrmManOperaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmManOperaciones_Load);
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
        private System.Windows.Forms.TextBox TxtIdOperacion;
        private System.Windows.Forms.Label LblIdOperacion;
        private System.Windows.Forms.Label LblClaveOperacion;
        private System.Windows.Forms.TextBox TxtClaveOperacion;
        private System.Windows.Forms.TextBox TxtDescripcion;
        private System.Windows.Forms.Label LblOperacion;
        private System.Windows.Forms.TextBox TxtLlamada;
        private System.Windows.Forms.Label LblLamada;
        private System.Windows.Forms.TextBox TxtRuta;
        private System.Windows.Forms.Label LblRuta;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox cmbEstatus;
    }
}
