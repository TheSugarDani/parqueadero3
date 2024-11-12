using IronOcr;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Linq;
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

        private Dictionary<string, DateTime> registroPlacas = new Dictionary<string, DateTime>();

        public VistaPersonita()
        {
            InitializeComponent();
            InitializeCamera();

            // Inicializa y configura IronTesseract
            ocrEngine = new IronTesseract();
            ocrEngine.Language = OcrLanguage.Spanish;

            // Configuración adicional del OCR
            ocrEngine.Configuration.WhiteListCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            ocrEngine.Configuration.BlackListCharacters = "~`$#^*_}{][|\\-@'.&%!:;,)(=+";
            ocrEngine.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.SingleLine;
        }

        private void InitializeCamera()
        {
            capture = new VideoCapture(0); // Iniciar la cámara

            if (!capture.IsOpened())
            {
                MessageBox.Show("No se pudo abrir la cámara.");
                return;
            }

            cameraTimer = new System.Windows.Forms.Timer();
            cameraTimer.Interval = 100; // Intervalo para tomar imagen
            cameraTimer.Tick += CameraTimer_Tick;
            cameraTimer.Start();

            limpiezaTimer = new System.Windows.Forms.Timer();
            limpiezaTimer.Interval = 5000; // Espera 5 segundos antes de limpiar el registro
            limpiezaTimer.Tick += LimpiarRegistro_Tick;
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame); // Captura la imagen 
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

                    // Procesa la imagen solo si no está procesando otra
                    if (!_isProcessing)
                    {
                        _isProcessing = true;
                        CapturarPlaca(frame); // Procesa OCR de la imagen capturada
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
                    // Preprocesamiento de la imagen: Convertir a escala de grises
                    Mat grayFrame = new Mat();
                    Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY);

                    // Mejorar el contraste con ecualización de histograma
                    Cv2.EqualizeHist(grayFrame, grayFrame);
                     
                    // Umbralización adaptativa para manejar cambios de luz
                    Mat thresholdFrame = new Mat();
                    Cv2.AdaptiveThreshold(grayFrame, thresholdFrame, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 11, 2);

                    // Filtro de mediana para reducir el ruido en la imagen
                    Mat cleanFrame = new Mat();
                    Cv2.MedianBlur(thresholdFrame, cleanFrame, 3);

                    // Recortar la imagen para enfocarse solo en la zona de la placa
                    Rect placaRect = new Rect(50, 50, frame.Width - 100, 100); // Ajusta el área donde esperas la placa
                    Mat placaFrame = cleanFrame[placaRect];

                    using (var captura = placaFrame.ToBitmap())
                    {
                        var resultadoOCR = ocrEngine.Read(captura); // Ejecutar OCR

                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();

                            // Validación de placa (tiene 6 caracteres alfanuméricos)
                            if (EsPlacaValida(placaDetectada))
                            {
                                // Primera detección: registrar hora de ingreso
                                if (!placaIngresada)
                                {
                                    placaActual = placaDetectada;
                                    horaIngreso = DateTime.Now;
                                    placaIngresada = true;

                                    // Actualizar los labels con la placa y la hora de ingreso
                                    lblPlaca.Text = $"Placa: {placaActual}";
                                    labelHoraIngreso.Text = $"Hora de Ingreso: {horaIngreso:HH:mm:ss}";

                                    // Guardar en el diccionario con la hora de ingreso
                                    if (!registroPlacas.ContainsKey(placaActual))
                                    {
                                        registroPlacas.Add(placaActual, horaIngreso);
                                    }

                                    // Iniciar el temporizador para limpiar los registros después de 5 segundos
                                    limpiezaTimer.Start();
                                }
                                // Si la placa ya está ingresada, registrar hora de salida
                                else if (placaActual == placaDetectada)
                                {
                                    horaSalida = DateTime.Now;
                                    labelHoraSalida.Text = $"Hora de Salida: {horaSalida:HH:mm:ss}";

                                    // Guardar la hora de salida en el diccionario
                                    if (registroPlacas.ContainsKey(placaActual))
                                    {
                                        registroPlacas[placaActual] = horaSalida;
                                    }

                                    // Detener el temporizador si se detecta la placa registrada para la salida
                                    limpiezaTimer.Start();
                                }
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
                    _isProcessing = false; // Termina el procesamiento
                }
            }
        }

        private bool EsPlacaValida(string placa)
        {
            // Validación más estricta: La placa debe tener exactamente 6 caracteres y solo alfanuméricos
            return placa.Length == 6 && placa.All(char.IsLetterOrDigit);
        }

        private void LimpiarRegistro_Tick(object sender, EventArgs e)
        {
            // Limpiar los registros después de que haya pasado el tiempo de espera de 5 segundos
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

        private void lblPlaca_Click(object sender, EventArgs e)
        {
            // Acción al hacer clic en lblPlaca si se requiere
        }
    }
}
