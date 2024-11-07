using IronOcr;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace parqueadero2
{
    public partial class VistaPersonita : Form
    {
        private IronTesseract ocrEngine;
        private VideoCapture capture;
        private string placaActual = string.Empty;
        private DateTime horaIngreso;
        private DateTime horaSalida;
        private bool placaIngresada = false;
        private System.Windows.Forms.Timer cameraTimer;

        public VistaPersonita()
        {
            InitializeComponent();
            ocrEngine = new IronTesseract();
            ocrEngine.Language = OcrLanguage.Spanish; 
        }

        private void VistaPersonita_Load(object sender, EventArgs e)
        {
            try
            {
                capture = new VideoCapture(0); 

                if (!capture.IsOpened()) 
                {
                    return;
                }

                cameraTimer = new System.Windows.Forms.Timer();
                cameraTimer.Interval = 100; 
                cameraTimer.Tick += CameraTimer_Tick;
                cameraTimer.Start(); 
            }
            catch (Exception ex)
            {

            }
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame); 
                    if (frame.Empty())
                    {
                        return; 
                    }


                    if (pictureBoxPlaca.Image != null)
                    {
                        pictureBoxPlaca.Image.Dispose();
                    }

                    pictureBoxPlaca.Image = BitmapConverter.ToBitmap(frame);
                    pictureBoxPlaca.Refresh();


                    CapturarPlaca(frame);
                }
            }
            catch
            {

            }
        }

        private void CapturarPlaca(Mat frame)
        {
            if (frame != null)
            {
                try
                {
                    using (var captura = frame.ToBitmap())
                    {
                        var resultadoOCR = ocrEngine.Read(captura);
                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();
                            if (!placaIngresada)
                            {
                                placaActual = placaDetectada;
                                horaIngreso = DateTime.Now;
                                placaIngresada = true;

                                labelPlaca.Text = $"Placa: {placaActual}";
                                labelHoraIngreso.Text = $"Hora de Ingreso: {horaIngreso:HH:mm:ss}";
                            }
                            else if (placaActual == placaDetectada)
                            {
                                horaSalida = DateTime.Now;
                                labelHoraSalida.Text = $"Hora de Salida: {horaSalida:HH:mm:ss}";
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void VistaPersonita_FormClosing(object sender, FormClosingEventArgs e)
        {

            cameraTimer?.Stop();
            capture?.Release();
        }

        private void labelPlaca_Click(object sender, EventArgs e)
        {

        }
    }
}
