using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class Program
    {
        static async Task Main(string[] args)
        {            
            // Create MQTT client.
            var mqttClient = new MqttClient("doranhome/pc/main/winmedia");

            try
            {
                await mqttClient.Connect();
                Console.WriteLine("Connected to MQTT.");
            }
            catch (ConnectivityError)
            {
                Console.WriteLine("Unable to connect to MQTT broker.");
            }


            Console.WriteLine("Initialize media.");

            // Create media controls
            var mediaControls = new MediaControls();
            await mediaControls.Initialize();

            // Create link between MQTT client and media controls.
            var eventHandler = new MediaControlsEventHandler(mediaControls, mqttClient);
            eventHandler.Initialize();

            // Dumb infinite loop to keep application running while listening to events.
            // TODO: Use a better approach.
            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
