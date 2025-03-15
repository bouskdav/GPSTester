using Ghostware.GPS.NET;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

namespace GPSTester
{
    internal class Program
    {
        private static GpsService? _gpsService;

        static void Main(string[] args)
        {
            Console.WriteLine("GPSTester");

            var gpsdClient = new GpsdClient();

            if (!gpsdClient.CheckGpsdRunning())
                gpsdClient.StartGpsd();

            var info = new GpsdInfo()
            {
                Address = "127.0.0.1",
                //Default
                Port = 2947,
                IsProxyEnabled = false,
                //ProxyAddress = "proxy",
                //ProxyPort = 80,
                //ProxyCredentials = new ProxyCredentials("*****", "*****")
            };
            _gpsService = new GpsService(info);

            _gpsService.RegisterDataEvent(GpsdServiceOnLocationChanged);
            _gpsService.Connect();

            while (true)
            {
                string? input = Console.ReadLine();

                if (String.IsNullOrEmpty(input))
                    continue;

                //if (input == "start")
                //    await gpsdClient.RunAsync();

                //if (input == "start")
                //    await gpsdClient.RunAsync();

                if (input == "exit")
                    break;
            }

            gpsdClient.StopGpsd();
        }

        private static void GpsdServiceOnLocationChanged(object sender, GpsDataEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
