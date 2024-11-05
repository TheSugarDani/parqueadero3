namespace parqueadero2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblPlaca = new Label();
            lblHoraIngreso = new Label();
            label3 = new Label();
            lblHoraSalida = new Label();
            label5 = new Label();
            pictureBoxCamara = new PictureBox();
            CapturarPlaca = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxCamara).BeginInit();
            SuspendLayout();
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.Location = new Point(357, 26);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(35, 15);
            lblPlaca.TabIndex = 0;
            lblPlaca.Text = "Placa";
            // 
            // lblHoraIngreso
            // 
            lblHoraIngreso.AutoSize = true;
            lblHoraIngreso.Location = new Point(205, 296);
            lblHoraIngreso.Name = "lblHoraIngreso";
            lblHoraIngreso.Size = new Size(75, 15);
            lblHoraIngreso.TabIndex = 1;
            lblHoraIngreso.Text = "Hora Ingreso";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(217, 327);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 2;
            label3.Text = "Ingreso";
            // 
            // lblHoraSalida
            // 
            lblHoraSalida.AutoSize = true;
            lblHoraSalida.Location = new Point(512, 296);
            lblHoraSalida.Name = "lblHoraSalida";
            lblHoraSalida.Size = new Size(67, 15);
            lblHoraSalida.TabIndex = 3;
            lblHoraSalida.Text = "Hora Salida";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(525, 327);
            label5.Name = "label5";
            label5.Size = new Size(38, 15);
            label5.TabIndex = 4;
            label5.Text = "Salida";
            // 
            // pictureBoxCamara
            // 
            pictureBoxCamara.Location = new Point(205, 72);
            pictureBoxCamara.Name = "pictureBoxCamara";
            pictureBoxCamara.Size = new Size(358, 204);
            pictureBoxCamara.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxCamara.TabIndex = 5;
            pictureBoxCamara.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBoxCamara);
            Controls.Add(label5);
            Controls.Add(lblHoraSalida);
            Controls.Add(label3);
            Controls.Add(lblHoraIngreso);
            Controls.Add(lblPlaca);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBoxCamara).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPlaca;
        private Label lblHoraIngreso;
        private Label label3;
        private Label lblHoraSalida;
        private Label label5;
        private PictureBox pictureBoxCamara;
        private System.Windows.Forms.Timer CapturarPlaca;
    }
}
