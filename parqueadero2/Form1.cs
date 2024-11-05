using IronOcr; // Asegúrate de tener esta biblioteca instalada
using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace parqueadero2
{
    public partial class VistaPersonita : Form
    {
        private IronTesseract ocrEngine; // Motor OCR
        private FilterInfoCollection dispositivos;
        private VideoCaptureDevice camara;
        private string placaActual = string.Empty;
        private DateTime horaIngreso;
        private DateTime horaSalida;
        private bool placaIngresada = false;

        public VistaPersonita()
        {
            InitializeComponent();
            ocrEngine = new IronTesseract();
            ocrEngine.Language = OcrLanguage.Spanish; // Configura el idioma
        }

        private void VistaPersonita_Load(object sender, EventArgs e)
        {
            // Inicializa la cámara
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (dispositivos.Count > 0)
            {
                camara = new VideoCaptureDevice(dispositivos[0].MonikerString);
                camara.NewFrame += Camara_NewFrame; // Captura de nuevo frame
                camara.Start();
            }
            else
            {
                MessageBox.Show("No se encontraron cámaras.");
            }

            timer1.Interval = 1000; // Intervalo de 1 segundo
            timer1.Tick += Timer1_Tick; // Suscribe al evento Tick
            timer1.Start(); // Inicia el Timer
        }

        private void Camara_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Muestra el nuevo frame en el PictureBox
            if (pictureBoxPlaca.InvokeRequired)
            {
                pictureBoxPlaca.Invoke(new MethodInvoker(() =>
                {
                    // Clona el frame y lo asigna al PictureBox
                    pictureBoxPlaca.Image?.Dispose(); // Libera la imagen anterior
                    pictureBoxPlaca.Image = (Bitmap)eventArgs.Frame.Clone();
                }));
            }
            else
            {
                pictureBoxPlaca.Image?.Dispose(); // Libera la imagen anterior
                pictureBoxPlaca.Image = (Bitmap)eventArgs.Frame.Clone();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            CapturarPlaca();
        }

        private void CapturarPlaca()
        {
            if (pictureBoxPlaca.Image != null)
            {
                try
                {
                    using (var captura = new Bitmap(pictureBoxPlaca.Image))
                    {
                        var resultadoOCR = ocrEngine.Read(captura);
                        if (!string.IsNullOrEmpty(resultadoOCR.Text))
                        {
                            string placaDetectada = resultadoOCR.Text.Trim();
                            // Verifica si es una nueva placa
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
            // Detiene la cámara al cerrar el formulario
            if (camara != null && camara.IsRunning)
            {
                camara.SignalToStop();
                camara.WaitForStop();
            }
        }

        private void labelPlaca_Click(object sender, EventArgs e)
        {
            // Puedes manejar el evento clic aquí si es necesario
        }
    }
}
