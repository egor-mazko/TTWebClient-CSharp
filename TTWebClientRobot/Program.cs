using System;
using TTWebClient;
using TTWebRobot;

namespace TTWebClientRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: TTWebClientRobot.exe WebApiAddress WebApiId WebApiKey WebApiSecret");
                return;
            }

            string webApiAddress = args[0];
            string webApiId = args[1];
            string webApiKey = args[2];
            string webApiSecret = args[3];

            // Optional: Force to ignore server certificate 
            TickTraderWebClient.IgnoreServerCertificate();

            // Create instance of the TickTrader Web API client
            var client = new TickTraderWebClient(webApiAddress, webApiId, webApiKey, webApiSecret);

            // Create and run WebRobot
            var robot = new WebRobot(client);
            robot.RunInConsole();
        }
    }
}
