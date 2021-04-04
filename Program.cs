using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mqttClient = new MqttClient();

            try
            {
                await mqttClient.Connect();
                Console.WriteLine("Connected to MQTT.");
            } catch (ConnectivityError)
            {
                Console.WriteLine("Unable to connect to MQTT broker.");
            }
        }
    }
}
