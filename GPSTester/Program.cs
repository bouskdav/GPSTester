namespace GPSTester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("GPSTester");

            using var gpsdClient = new GpsdClient();

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "start")
                    await gpsdClient.RunAsync();

                if (input == "exit")
                    break;
            }
        }
    }
}
