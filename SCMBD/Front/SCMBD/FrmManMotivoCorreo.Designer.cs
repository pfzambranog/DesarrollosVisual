namespace SCMBD
{
    partial class FrmManMotivoCorreo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManMotivoCorreo));
            this.panel0 = new System.Windows.Forms.Panel();
            this.BtnBaja = new System.Windows.Forms.Button();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.PanelEncabezado = new System.Windows.Forms.Panel();
            this.cmbCompartir = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtPerfilCorreo = new System.Windows.Forms.TextBox();
            this.LblPerfilCorreo = new System.Windows.Forms.Label();
            this.TxtURL = new System.Windows.Forms.TextBox();
            this.LblURL = new System.Windows.Forms.Label();
            this.LblHTML = new System.Windows.Forms.Label();
            this.TxtHTML = new System.Windows.Forms.TextBox();
            this.LblCuerpo = new System.Windows.Forms.Label();
            this.TxtTitulo = new System.Windows.Forms.TextBox();
            this.cmbEstatus = new System.Windows.Forms.ComboBox();
            this.LblEstatus = new System.Windows.Forms.Label();
            this.TxtCuerpo = new System.Windows.Forms.TextBox();
            this.LblTitulo = new System.Windows.Forms.Label();
            this.TxtDescripcion = new System.Windows.Forms.TextBox();
            this.LblDescripcion = new System.Windows.Forms.Label();
            this.TxtIdMotivo = new System.Windows.Forms.TextBox();
            this.LblIdMotivo = new System.Windows.Forms.Label();
            this.panel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.PanelEncabezado.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel0
            // 
            this.panel0.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel0.Controls.Add(this.BtnBaja);
            this.panel0.Controls.Add(this.BtnProcesar);
            this.panel0.Controls.Add(this.BtnExcel);
            this.panel0.Controls.Add(this.BtnRefrescar);
            this.panel0.Controls.Add(this.BtnSalir);
            this.panel0.Controls.Add(this.txtUsuario);
            this.panel0.Controls.Add(this.txtOperacion);
            this.panel0.Controls.Add(this.picLogo);
            this.panel0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel0.Location = new System.Drawing.Point(0, 0);
            this.panel0.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel0.Name = "panel0";
            this.panel0.Size = new System.Drawing.Size(1500, 80);
            this.panel0.TabIndex = 2;
            // 
            // BtnBaja
            // 
            this.BtnBaja.Image = global::SCMBD.Properties.Resources.Elimina;
            this.BtnBaja.Location = new System.Drawing.Point(1258, 0);
            this.BtnBaja.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnBaja.Name = "BtnBaja";
            this.BtnBaja.Size = new System.Drawing.Size(59, 42);
            this.BtnBaja.TabIndex = 15;
            this.BtnBaja.UseVisualStyleBackColor = true;
            this.BtnBaja.Click += new System.EventHandler(this.BtnBaja_Click);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(1203, 0);
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
            this.BtnExcel.Location = new System.Drawing.Point(1315, 2);
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
            this.BtnRefrescar.Location = new System.Drawing.Point(1370, 2);
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
            this.BtnSalir.Location = new System.Drawing.Point(1429, 2);
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
            // PanelEncabezado
            // 
            this.PanelEncabezado.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelEncabezado.Controls.Add(this.cmbCompartir);
            this.PanelEncabezado.Controls.Add(this.label1);
            this.PanelEncabezado.Controls.Add(this.TxtPerfilCorreo);
            this.PanelEncabezado.Controls.Add(this.LblPerfilCorreo);
            this.PanelEncabezado.Controls.Add(this.TxtURL);
            this.PanelEncabezado.Controls.Add(this.LblURL);
            this.PanelEncabezado.Controls.Add(this.LblHTML);
            this.PanelEncabezado.Controls.Add(this.TxtHTML);
            this.PanelEncabezado.Controls.Add(this.LblCuerpo);
            this.PanelEncabezado.Controls.Add(this.TxtTitulo);
            this.PanelEncabezado.Controls.Add(this.cmbEstatus);
            this.PanelEncabezado.Controls.Add(this.LblEstatus);
            this.PanelEncabezado.Controls.Add(this.TxtCuerpo);
            this.PanelEncabezado.Controls.Add(this.LblTitulo);
            this.PanelEncabezado.Controls.Add(this.TxtDescripcion);
            this.PanelEncabezado.Controls.Add(this.LblDescripcion);
            this.PanelEncabezado.Controls.Add(this.TxtIdMotivo);
            this.PanelEncabezado.Controls.Add(this.LblIdMotivo);
            this.PanelEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelEncabezado.Location = new System.Drawing.Point(0, 80);
            this.PanelEncabezado.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PanelEncabezado.Name = "PanelEncabezado";
            this.PanelEncabezado.Size = new System.Drawing.Size(1500, 159);
            this.PanelEncabezado.TabIndex = 3;
            // 
            // cmbCompartir
            // 
            this.cmbCompartir.BackColor = System.Drawing.SystemColors.Control;
            this.cmbCompartir.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCompartir.FormattingEnabled = true;
            this.cmbCompartir.Location = new System.Drawing.Point(603, 125);
            this.cmbCompartir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCompartir.Name = "cmbCompartir";
            this.cmbCompartir.Size = new System.Drawing.Size(44, 23);
            this.cmbCompartir.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(481, 128);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Permite Compartir:";
            // 
            // TxtPerfilCorreo
            // 
            this.TxtPerfilCorreo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtPerfilCorreo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtPerfilCorreo.Location = new System.Drawing.Point(570, 88);
            this.TxtPerfilCorreo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtPerfilCorreo.MaxLength = 100;
            this.TxtPerfilCorreo.Name = "TxtPerfilCorreo";
            this.TxtPerfilCorreo.Size = new System.Drawing.Size(339, 23);
            this.TxtPerfilCorreo.TabIndex = 6;
            // 
            // LblPerfilCorreo
            // 
            this.LblPerfilCorreo.AutoSize = true;
            this.LblPerfilCorreo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPerfilCorreo.Location = new System.Drawing.Point(479, 96);
            this.LblPerfilCorreo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPerfilCorreo.Name = "LblPerfilCorreo";
            this.LblPerfilCorreo.Size = new System.Drawing.Size(81, 15);
            this.LblPerfilCorreo.TabIndex = 18;
            this.LblPerfilCorreo.Text = "Perfil Correo:";
            // 
            // TxtURL
            // 
            this.TxtURL.BackColor = System.Drawing.SystemColors.Control;
            this.TxtURL.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtURL.Location = new System.Drawing.Point(570, 52);
            this.TxtURL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtURL.MaxLength = 100;
            this.TxtURL.Name = "TxtURL";
            this.TxtURL.Size = new System.Drawing.Size(339, 23);
            this.TxtURL.TabIndex = 5;
            // 
            // LblURL
            // 
            this.LblURL.AutoSize = true;
            this.LblURL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblURL.Location = new System.Drawing.Point(479, 60);
            this.LblURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblURL.Name = "LblURL";
            this.LblURL.Size = new System.Drawing.Size(33, 15);
            this.LblURL.TabIndex = 16;
            this.LblURL.Text = "URL:";
            // 
            // LblHTML
            // 
            this.LblHTML.AutoSize = true;
            this.LblHTML.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblHTML.Location = new System.Drawing.Point(1, 60);
            this.LblHTML.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblHTML.Name = "LblHTML";
            this.LblHTML.Size = new System.Drawing.Size(43, 15);
            this.LblHTML.TabIndex = 15;
            this.LblHTML.Text = "HTML:";
            // 
            // TxtHTML
            // 
            this.TxtHTML.BackColor = System.Drawing.SystemColors.Control;
            this.TxtHTML.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtHTML.Location = new System.Drawing.Point(73, 57);
            this.TxtHTML.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtHTML.Multiline = true;
            this.TxtHTML.Name = "TxtHTML";
            this.TxtHTML.Size = new System.Drawing.Size(384, 98);
            this.TxtHTML.TabIndex = 4;
            // 
            // LblCuerpo
            // 
            this.LblCuerpo.AutoSize = true;
            this.LblCuerpo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCuerpo.Location = new System.Drawing.Point(944, 19);
            this.LblCuerpo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblCuerpo.Name = "LblCuerpo";
            this.LblCuerpo.Size = new System.Drawing.Size(91, 15);
            this.LblCuerpo.TabIndex = 13;
            this.LblCuerpo.Text = "Cuerpo Correo:";
            // 
            // TxtTitulo
            // 
            this.TxtTitulo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtTitulo.Location = new System.Drawing.Point(570, 16);
            this.TxtTitulo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtTitulo.MaxLength = 100;
            this.TxtTitulo.Name = "TxtTitulo";
            this.TxtTitulo.Size = new System.Drawing.Size(339, 23);
            this.TxtTitulo.TabIndex = 2;
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(712, 128);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(197, 23);
            this.cmbEstatus.TabIndex = 11;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEstatus.Location = new System.Drawing.Point(655, 128);
            this.LblEstatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEstatus.Name = "LblEstatus";
            this.LblEstatus.Size = new System.Drawing.Size(49, 15);
            this.LblEstatus.TabIndex = 10;
            this.LblEstatus.Text = "Estatus:";
            // 
            // TxtCuerpo
            // 
            this.TxtCuerpo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtCuerpo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtCuerpo.Location = new System.Drawing.Point(1043, 16);
            this.TxtCuerpo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtCuerpo.Multiline = true;
            this.TxtCuerpo.Name = "TxtCuerpo";
            this.TxtCuerpo.Size = new System.Drawing.Size(453, 98);
            this.TxtCuerpo.TabIndex = 3;
            // 
            // LblTitulo
            // 
            this.LblTitulo.AutoSize = true;
            this.LblTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitulo.Location = new System.Drawing.Point(479, 19);
            this.LblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblTitulo.Name = "LblTitulo";
            this.LblTitulo.Size = new System.Drawing.Size(83, 15);
            this.LblTitulo.TabIndex = 8;
            this.LblTitulo.Text = "Titulo Correo:";
            // 
            // TxtDescripcion
            // 
            this.TxtDescripcion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtDescripcion.Location = new System.Drawing.Point(259, 11);
            this.TxtDescripcion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtDescripcion.MaxLength = 100;
            this.TxtDescripcion.Name = "TxtDescripcion";
            this.TxtDescripcion.Size = new System.Drawing.Size(198, 23);
            this.TxtDescripcion.TabIndex = 1;
            // 
            // LblDescripcion
            // 
            this.LblDescripcion.AutoSize = true;
            this.LblDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblDescripcion.Location = new System.Drawing.Point(160, 19);
            this.LblDescripcion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblDescripcion.Name = "LblDescripcion";
            this.LblDescripcion.Size = new System.Drawing.Size(91, 15);
            this.LblDescripcion.TabIndex = 2;
            this.LblDescripcion.Text = "Motivo Correo:";
            // 
            // TxtIdMotivo
            // 
            this.TxtIdMotivo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtIdMotivo.Enabled = false;
            this.TxtIdMotivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtIdMotivo.Location = new System.Drawing.Point(73, 16);
            this.TxtIdMotivo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtIdMotivo.MaxLength = 5;
            this.TxtIdMotivo.Name = "TxtIdMotivo";
            this.TxtIdMotivo.Size = new System.Drawing.Size(58, 23);
            this.TxtIdMotivo.TabIndex = 0;
            // 
            // LblIdMotivo
            // 
            this.LblIdMotivo.AutoSize = true;
            this.LblIdMotivo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblIdMotivo.Location = new System.Drawing.Point(1, 19);
            this.LblIdMotivo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblIdMotivo.Name = "LblIdMotivo";
            this.LblIdMotivo.Size = new System.Drawing.Size(64, 15);
            this.LblIdMotivo.TabIndex = 0;
            this.LblIdMotivo.Text = "Id Motivo:";
            // 
            // FrmManMotivoCorreo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1500, 780);
            this.Controls.Add(this.PanelEncabezado);
            this.Controls.Add(this.panel0);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManMotivoCorreo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmManMotivoCorreo_Load);
            this.panel0.ResumeLayout(false);
            this.panel0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.PanelEncabezado.ResumeLayout(false);
            this.PanelEncabezado.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel0;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtOperacion;
        private System.Windows.Forms.Button BtnRefrescar;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.Button BtnExcel;
        private System.Windows.Forms.Button BtnProcesar;
        private System.Windows.Forms.Panel PanelEncabezado;
        private System.Windows.Forms.TextBox TxtIdMotivo;
        private System.Windows.Forms.Label LblIdMotivo;
        private System.Windows.Forms.Label LblDescripcion;
        private System.Windows.Forms.TextBox TxtDescripcion;
        private System.Windows.Forms.TextBox TxtCuerpo;
        private System.Windows.Forms.Label LblTitulo;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox cmbEstatus;
        private System.Windows.Forms.Button BtnBaja;
        private System.Windows.Forms.TextBox TxtTitulo;
        private System.Windows.Forms.Label LblCuerpo;
        private System.Windows.Forms.Label LblHTML;
        private System.Windows.Forms.TextBox TxtHTML;
        private System.Windows.Forms.Label LblURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtPerfilCorreo;
        private System.Windows.Forms.Label LblPerfilCorreo;
        private System.Windows.Forms.TextBox TxtURL;
        private System.Windows.Forms.ComboBox cmbCompartir;
    }
}
