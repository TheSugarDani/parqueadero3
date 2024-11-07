using IronOcr;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace parqueadero2
{
    public partial class VistaPersonita : Form
    {
        bool result = IronOcr.License.IsValidLicense("IRONSUITE.DANIEL.CALENO.CUN.EDU.CO.5139-799DFF9641-AL6W6P7XOMY3GFBC-IHT5FSLOFTVG-KOACR2FAFODB-S2W74LBKXFX4-LF4BOGQMC4HR-IXZFNBFM3QSZ-FA7G62-TQQTI2WKJA2OEA-DEPLOYMENT.TRIAL-Q6B6RI.TRIAL.EXPIRES.28.NOV.2024");

        private IronTesseract ocrEngine;
        private VideoCapture capture;
        private string placaActual = string.Empty;
        private DateTime horaIngreso;
        private DateTime horaSalida;
        private bool placaIngresada = false;
        private bool _isProcessing = false; // Controla el procesamiento de la imagen
        private System.Windows.Forms.Timer cameraTimer;

        public VistaPersonita()
        {
            InitializeComponent();
            InitializeCamera();
            ocrEngine = new IronTesseract();
            ocrEngine.Language = OcrLanguage.Spanish;
        }

        private void InitializeCamera()
        {
            capture = new VideoCapture(0); // Inicializar la cámara

            if (!capture.IsOpened())
            {
                MessageBox.Show("No se pudo abrir la cámara.");
                return;
            }

            cameraTimer = new System.Windows.Forms.Timer();
            cameraTimer.Interval = 100; // Intervalo para tomar fotogramas
            cameraTimer.Tick += CameraTimer_Tick;
            cameraTimer.Start();
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame); // Capturar el fotograma
                    if (frame.Empty())
                    {
                        MessageBox.Show("No se pudo capturar un fotograma de la cámara.");
                        return;
                    }

                    // Actualización de la imagen en el PictureBox
                    if (pictureBoxPlaca.Image != null)
                    {
                        pictureBoxPlaca.Image.Dispose();
                    }

                    pictureBoxPlaca.Image = BitmapConverter.ToBitmap(frame);
                    pictureBoxPlaca.Refresh();

                    // Procesar la imagen solo si no se está procesando otra
                    if (!_isProcessing)
                    {
                        _isProcessing = true;
                        CapturarPlaca(frame); // Procesar OCR de la imagen capturada
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la actualización de la cámara: {ex.Message}");
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
                        var resultadoOCR = ocrEngine.Read(captura); // Ejecutar OCR
                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();
                            if (!placaIngresada)
                            {
                                placaActual = placaDetectada;
                                horaIngreso = DateTime.Now;
                                placaIngresada = true;

                                // Actualizar los labels con la placa y la hora de ingreso
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
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al capturar la placa: {ex.Message}");
                }
                finally
                {
                    _isProcessing = false; // Terminar procesamiento
                }
            }
        }

        private void VistaPersonita_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cameraTimer?.Stop(); // Detener el temporizador
                capture?.Release(); // Liberar la cámara
                                    // ocrEngine?.Dispose(); // No es necesario llamar a Dispose en IronTesseract
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la aplicación: {ex.Message}");
            }
        }

    }
}
