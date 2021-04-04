﻿using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class Program
    {
        static async Task Main(string[] args)
        {            
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
            var mediaControls = new MediaControls();
            await mediaControls.Initialize();


            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
