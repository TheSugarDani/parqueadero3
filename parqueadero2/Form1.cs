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
        private bool _isProcessing = false; // Control de procesamiento
        private System.Windows.Forms.Timer cameraTimer;
        private System.Windows.Forms.Timer limpiezaTimer; // Temporizador para limpiar los registros

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

            limpiezaTimer = new System.Windows.Forms.Timer();
            limpiezaTimer.Interval = 5000; // Esperar 5 segundos antes de limpiar los registros
            limpiezaTimer.Tick += LimpiarRegistro_Tick;
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
                    // Preprocesamiento de la imagen (Escala de grises y umbralización)
                    Mat grayFrame = new Mat();
                    Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY); // Convertir a escala de grises

                    // Umbralización
                    Mat thresholdFrame = new Mat();
                    Cv2.Threshold(grayFrame, thresholdFrame, 128, 255, ThresholdTypes.Binary);

                    // Filtrar el ruido
                    Mat cleanFrame = new Mat();
                    Cv2.MedianBlur(thresholdFrame, cleanFrame, 3);

                    using (var captura = cleanFrame.ToBitmap())
                    {
                        var resultadoOCR = ocrEngine.Read(captura); // Ejecutar OCR
                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();

                            // Primera detección: registrar ingreso
                            if (!placaIngresada)
                            {
                                placaActual = placaDetectada;
                                horaIngreso = DateTime.Now;
                                placaIngresada = true;

                                // Actualizar los labels con la placa y la hora de ingreso
                                lblPlaca.Text = $"Placa: {placaActual}";
                                labelHoraIngreso.Text = $"Hora de Ingreso: {horaIngreso:HH:mm:ss}";

                                // Iniciar el temporizador para limpiar los registros después de 5 segundos
                                limpiezaTimer.Start();
                            }
                            // Si la placa ya está ingresada, registrar salida
                            else if (placaActual == placaDetectada)
                            {
                                horaSalida = DateTime.Now;
                                labelHoraSalida.Text = $"Hora de Salida: {horaSalida:HH:mm:ss}";

                                // Detener el temporizador si se detecta la placa registrada para la salida
                                limpiezaTimer.Stop();

                                // Llamar a la función de limpieza para permitir una nueva detección de placa
                                LimpiarRegistro();
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

        private void LimpiarRegistro_Tick(object sender, EventArgs e)
        {
            // Limpiar los registros después de que haya pasado el tiempo de espera (5 segundos)
            LimpiarRegistro();
            // Detener el temporizador después de limpiar los registros
            limpiezaTimer.Stop();
        }

        private void LimpiarRegistro()
        {
            // Reiniciar variables para permitir una nueva detección
            placaActual = string.Empty;
            placaIngresada = false;

            // Limpiar los labels para el próximo vehículo
            lblPlaca.Text = "Placa:";
            labelHoraIngreso.Text = "Hora de Ingreso:";
            labelHoraSalida.Text = "Hora de Salida:";

            // Opcional: Limpiar la imagen del PictureBox
            pictureBoxPlaca.Image = null;
        }

        private void VistaPersonita_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cameraTimer?.Stop(); // Detener el temporizador
                capture?.Release(); // Liberar la cámara
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cerrar la aplicación: {ex.Message}");
            }
        }
    }
}
