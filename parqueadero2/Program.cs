namespace parqueadero2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IronOcr.License.LicenseKey = "IRONSUITE.DANIEL.CALENO.CUN.EDU.CO.5139-799DFF9641-AL6W6P7XOMY3GFBC-IHT5FSLOFTVG-KOACR2FAFODB-S2W74LBKXFX4-LF4BOGQMC4HR-IXZFNBFM3QSZ-FA7G62-TQQTI2WKJA2OEA-DEPLOYMENT.TRIAL-Q6B6RI.TRIAL.EXPIRES.28.NOV.2024";

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new VistaPersonita());
        }
    }
}