using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GPSTester
{
    public class GpsdClient : IDisposable
    {
        private const string Host = "127.0.0.1"; // Change if GPSD is on another host
        private const int Port = 2947;

        public void Dispose()
        {
            StopGpsd();
        }

        public async Task RunAsync()
        {
            try
            {
                var gpsdIsRunning = CheckGpsdRunning();

                if (!gpsdIsRunning)
                {
                    StartGpsd();
                }

                using TcpClient client = new TcpClient();
                await client.ConnectAsync(Host, Port);
                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
                using StreamReader reader = new StreamReader(stream, Encoding.ASCII);

                // Enable JSON mode
                await writer.WriteLineAsync("?WATCH={\"enable\":true,\"json\":true}");

                while (true)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line == null) break;

                    try
                    {
                        var gpsData = JsonSerializer.Deserialize<GpsdResponse>(line);
                        if (gpsData != null && gpsData.Class == "TPV")
                        {
                            Console.WriteLine($"Time: {gpsData.Time}");
                            Console.WriteLine($"Latitude: {gpsData.Lat}");
                            Console.WriteLine($"Longitude: {gpsData.Lon}");
                            Console.WriteLine($"Altitude: {gpsData.Alt}");
                            Console.WriteLine("-----------------------");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"JSON Parse Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private bool CheckGpsdRunning()
        {
            string command = "pgrep gpsd";
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }

            return !String.IsNullOrEmpty(result);
        }

        private void StartGpsd()
        {
            string command = "gpsd -nG -s 115200 /dev/ttyACM0";
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }

            //return !String.IsNullOrEmpty(result);
        }

        private void StopGpsd()
        {
            string command = "pkill gpsd";
            string result = "";
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + command + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();

                proc.WaitForExit();
            }

            //return !String.IsNullOrEmpty(result);
        }
    }

    public class GpsdResponse
    {
        public string? Class { get; set; }
        public string? Time { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public double? Alt { get; set; }
    }
}
