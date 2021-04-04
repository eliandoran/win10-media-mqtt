using MQTTnet;
using MQTTnet.Client;
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
        private IMqttClient client;

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
                this.client = client;
            } catch (MqttCommunicationException e)
            {
                throw new ConnectivityError();
            }
        }

        public async Task Publish(string subtopic, string payload)
        {
           if (client == null)
            {
                throw new ConnectivityError();
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic + "/" + subtopic)
                .WithPayload(payload)
                .Build();
            
            await client.PublishAsync(message);
        }

    }
}
