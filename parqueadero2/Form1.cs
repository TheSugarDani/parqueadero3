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
        private bool _isProcessing = false; // control de procesamiento
        private System.Windows.Forms.Timer cameraTimer;
        private System.Windows.Forms.Timer limpiezaTimer; // temporizador para limpiar los registros

        public VistaPersonita()
        {
            InitializeComponent();
            InitializeCamera();
            ocrEngine = new IronTesseract();
            ocrEngine.Language = OcrLanguage.Spanish;
        }

        private void InitializeCamera()
        {
            IronTesseract ocr = new IronTesseract();

            // Configure for speed
            ocr.Configuration.BlackListCharacters = "~`$#^*_}{][|\\-@'.&%!:;,)(=+";
            ocr.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.Auto;
            ocr.Language = OcrLanguage.EnglishFast;

            using OcrInput input = new OcrInput();
            var Pageindices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            OcrResult resul = ocr.Read(input);
            Console.WriteLine(resul.Text);


            capture = new VideoCapture(0); // iniciar la cámara

            if (!capture.IsOpened())
            {
                MessageBox.Show("No se pudo abrir la cámara.");
                return;
            }

            cameraTimer = new System.Windows.Forms.Timer();
            cameraTimer.Interval = 100; // intervalo para tomar imagen
            cameraTimer.Tick += CameraTimer_Tick;
            cameraTimer.Start();

            limpiezaTimer = new System.Windows.Forms.Timer();
            limpiezaTimer.Interval = 5000; // espera 5 segundos antes de limpiar el registro
            limpiezaTimer.Tick += LimpiarRegistro_Tick;
        }

        private void CameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame); // captura la imagen 
                    if (frame.Empty())
                    {
                        MessageBox.Show("No se pudo capturar un fotograma de la cámara.");
                        return;
                    }

                    // actualizacion de imagen en el picturebox
                    if (pictureBoxPlaca.Image != null)
                    {
                        pictureBoxPlaca.Image.Dispose();
                    }

                    pictureBoxPlaca.Image = BitmapConverter.ToBitmap(frame);
                    pictureBoxPlaca.Refresh();

                    // procesa la imagen solo si no esta procesando otra
                    if (!_isProcessing)
                    {
                        _isProcessing = true;
                        CapturarPlaca(frame); // procesa ocr de la imagen capturada
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
                    // preprocesamiento de la imagen escala de grises y umbralización
                    Mat grayFrame = new Mat();
                    Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY); // convertir a escala de grises

                    // umbralizacion
                    Mat thresholdFrame = new Mat();
                    Cv2.Threshold(grayFrame, thresholdFrame, 128, 255, ThresholdTypes.Binary);

                    // filtrar el ruido
                    Mat cleanFrame = new Mat();
                    Cv2.MedianBlur(thresholdFrame, cleanFrame, 3);

                    using (var captura = cleanFrame.ToBitmap())
                    {
                        var resultadoOCR = ocrEngine.Read(captura); // ejecutar ocr
                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();

                            // primera detección, registra ingreso
                            if (!placaIngresada)
                            {
                                placaActual = placaDetectada;
                                horaIngreso = DateTime.Now;
                                placaIngresada = true;

                                // actualiza los labels con la placa y la hora de ingreso
                                lblPlaca.Text = $"Placa: {placaActual}";
                                labelHoraIngreso.Text = $"Hora de Ingreso: {horaIngreso:HH:mm:ss}";

                                // iniciar el temporizador para limpiar los registros despues de 5 segundos
                                limpiezaTimer.Start();
                            }
                            // si la placa ya esta ingresada, registra salida
                            else if (placaActual == placaDetectada)
                            {
                                horaSalida = DateTime.Now;
                                labelHoraSalida.Text = $"Hora de Salida: {horaSalida:HH:mm:ss}";

                                // detener el temporizador si se detecta la placa registrada para la salida
                                limpiezaTimer.Start();

                                // llamar a la funcion de limpieza para permitir una nueva deteccion de placa
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
                    _isProcessing = false; // termina procesamiento
                }
            }
        }

        private void LimpiarRegistro_Tick(object sender, EventArgs e)
        {
            // limpia los registros despues de que haya pasado el tiempo de espera de 5 segundos
            LimpiarRegistro();
            // detener el temporizador despues de limpiar los registros
            limpiezaTimer.Stop();
        }

        private void LimpiarRegistro()
        {
            // reiniar variables para permitir una nueva deteccion
            placaActual = string.Empty;
            placaIngresada = false;

            // limpia los labels para el proximo vehiculo
            lblPlaca.Text = "Placa:";
            labelHoraIngreso.Text = "Hora de Ingreso:";
            labelHoraSalida.Text = "Hora de Salida:";

            // opcional, limpia la imagen del picturebox
            pictureBoxPlaca.Image = null;
        }

        

        private void lblPlaca_Click(object sender, EventArgs e)
        {

        }

        private void VistaPersonita_Load(object sender, EventArgs e)
        {

        }
    }

}