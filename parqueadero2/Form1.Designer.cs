namespace parqueadero2
{
    partial class VistaPersonita
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
            components = new System.ComponentModel.Container();
            labelPlaca = new Label();
            labelHoraIngreso = new Label();
            labelHoraSalida = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            pictureBoxPlaca = new PictureBox();
            lblPlaca = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlaca).BeginInit();
            SuspendLayout();
            // 
            // labelPlaca
            // 
            labelPlaca.AutoSize = true;
            labelPlaca.Location = new Point(351, 30);
            labelPlaca.Name = "labelPlaca";
            labelPlaca.Size = new Size(0, 15);
            labelPlaca.TabIndex = 0;
            // 
            // labelHoraIngreso
            // 
            labelHoraIngreso.AutoSize = true;
            labelHoraIngreso.Location = new Point(195, 368);
            labelHoraIngreso.Name = "labelHoraIngreso";
            labelHoraIngreso.Size = new Size(94, 15);
            labelHoraIngreso.TabIndex = 2;
            labelHoraIngreso.Text = "Hora de Ingreso:";
            // 
            // labelHoraSalida
            // 
            labelHoraSalida.AutoSize = true;
            labelHoraSalida.Location = new Point(468, 368);
            labelHoraSalida.Name = "labelHoraSalida";
            labelHoraSalida.Size = new Size(86, 15);
            labelHoraSalida.TabIndex = 3;
            labelHoraSalida.Text = "Hora de Salida:";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            // 
            // pictureBoxPlaca
            // 
            pictureBoxPlaca.Location = new Point(229, 65);
            pictureBoxPlaca.Name = "pictureBoxPlaca";
            pictureBoxPlaca.Size = new Size(294, 269);
            pictureBoxPlaca.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPlaca.TabIndex = 4;
            pictureBoxPlaca.TabStop = false;
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.Location = new Point(351, 30);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(38, 15);
            lblPlaca.TabIndex = 5;
            lblPlaca.Text = "Placa:";
            // 
            // VistaPersonita
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblPlaca);
            Controls.Add(pictureBoxPlaca);
            Controls.Add(labelHoraSalida);
            Controls.Add(labelHoraIngreso);
            Controls.Add(labelPlaca);
            Name = "VistaPersonita";
            Text = "administrador";
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlaca).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPlaca;
        private Label labelHoraIngreso;
        private Label labelHoraSalida;
        private System.Windows.Forms.Timer timer1;
        private PictureBox pictureBoxPlaca;
        private Label lblPlaca;
    }
}