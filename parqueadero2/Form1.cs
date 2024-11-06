using IronOcr;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
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
            ocrEngine.Language = OcrLanguage.Spanish; // Configura el idioma
        }

        private void VistaPersonita_Load(object sender, EventArgs e)
        {
            capture = new VideoCapture(0); // Usar la cámara predeterminada
            if (!capture.IsOpened())
            {
                MessageBox.Show("No se pudo abrir la cámara.");
                return;
            }

            // Obtener las propiedades de la cámara usando índices enteros
            double width = capture.Get(3);  // 3 es el índice para el ancho del frame
            double height = capture.Get(4); // 4 es el índice para la altura del frame
            MessageBox.Show($"Cámara abierta con resolución: {width}x{height}");

            // Configura el Timer para actualizar la imagen
            cameraTimer = new System.Windows.Forms.Timer();
            cameraTimer.Interval = 100; // Intervalo de 100 ms
            cameraTimer.Tick += CameraTimer_Tick;
            cameraTimer.Start(); // Inicia el Timer
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);  // Captura un frame de la cámara
                    if (frame.Empty())
                    {
                        MessageBox.Show("No se pudo capturar un frame.");
                    }
                    else
                    {
                        // Libera la imagen anterior del PictureBox antes de mostrar una nueva
                        if (pictureBoxPlaca.Image != null)
                        {
                            pictureBoxPlaca.Image.Dispose();
                        }

                        // Convertir el frame capturado a Bitmap y mostrar en el PictureBox
                        pictureBoxPlaca.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
                        pictureBoxPlaca.Refresh(); // Refresca el PictureBox

                        // Procesa la imagen para detectar la placa
                        CapturarPlaca(frame);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturando el frame: {ex.Message}");
            }
        }

        private void CapturarPlaca(Mat frame)
        {
            if (frame != null)
            {
                try
                {
                    // Usar IronOCR para procesar la imagen capturada
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
                                labelHoraIngreso.Text = $"Hora de Ingreso: {horaIngreso.ToString("HH:mm:ss")}";
                            }
                            else if (placaActual == placaDetectada)
                            {
                                horaSalida = DateTime.Now;
                                labelHoraSalida.Text = $"Hora de Salida: {horaSalida.ToString("HH:mm:ss")}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al capturar la placa: {ex.Message}");
                }
            }
        }

        private void VistaPersonita_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Libera los recursos de la cámara al cerrar el formulario
            cameraTimer?.Stop();
            capture?.Release();
        }
    }
}
