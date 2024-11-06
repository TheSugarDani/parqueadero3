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
            ocrEngine.Language = OcrLanguage.Spanish; // Configura el idioma
        }

        private void VistaPersonita_Load(object sender, EventArgs e)
        {
            try
            {
                capture = new VideoCapture(0); // Usar la cámara predeterminada (índice 0)

                if (!capture.IsOpened()) // Verifica si la cámara se abre correctamente
                {
                    // Si la cámara no se inicia, salir del método
                    return;
                }

                // Configura el Timer para actualizar la imagen
                cameraTimer = new System.Windows.Forms.Timer();
                cameraTimer.Interval = 100; // Intervalo de 100 ms
                cameraTimer.Tick += CameraTimer_Tick;
                cameraTimer.Start(); // Inicia el Timer
            }
            catch (Exception ex)
            {
                // Puedes agregar un log de error si lo deseas
            }
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
                        return; // Si el frame está vacío, salir
                    }

                    // Actualizar el PictureBox con el frame capturado
                    if (pictureBoxPlaca.Image != null)
                    {
                        pictureBoxPlaca.Image.Dispose();
                    }

                    pictureBoxPlaca.Image = BitmapConverter.ToBitmap(frame);
                    pictureBoxPlaca.Refresh();

                    // Llamar a la detección de placa en el frame actual
                    CapturarPlaca(frame);
                }
            }
            catch
            {
                // Silenciar cualquier excepción aquí si prefieres no mostrar mensajes
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
                    // Silenciar cualquier excepción aquí si prefieres no mostrar mensajes
                }
            }
        }

        private void VistaPersonita_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Libera los recursos de la cámara y el timer al cerrar el formulario
            cameraTimer?.Stop();
            capture?.Release();
        }
    }
}
