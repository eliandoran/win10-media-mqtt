using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UwpCompanion
{
    class MqttClient
    {

        private string topic;

        public MqttClient(string topic)
        {
            this.topic = topic;
        }


        public async Task Connect()
        {
            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.0.50", 1883)
                .Build();

            try
            {
                await client.ConnectAsync(options, CancellationToken.None);
            } catch (MqttCommunicationException e)
            {
                throw new ConnectivityError();
            }
        }

    }
}
