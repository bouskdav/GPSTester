namespace GPSTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GPSTester");

            using var gpsdClient = new GpsdClient();

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "start")
                    gpsdClient.RunAsync();

                if (input == "exit")
                    break;
            }
        }
    }
}
