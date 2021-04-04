using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Exceptions;
using MQTTnet.Extensions.ManagedClient;
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
        private IManagedMqttClient client;

        public MqttClient(string topic)
        {
            this.topic = topic;
        }


        public async Task Connect()
        {
            var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithTcpServer("192.168.0.50", 1883)
                        .Build())
                    .Build();

            var client = new MqttFactory().CreateManagedMqttClient();
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnect);
            client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnect);
            client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnApplicationMessageReceived);
            await client.StartAsync(options);

            this.client = client;
        }

        private async void OnConnect(MqttClientConnectedEventArgs e)
        {
            await Subscribe("command");
            Console.WriteLine("Connect");
        }

        private void OnDisconnect(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine("Disconnect");
        }

        private void OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            OnMessageReceived(new MessageReceivedEventArgs()
            {
                Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload),
                Topic = e.ApplicationMessage.Topic
            });
        }

        public async Task Subscribe(string subtopic)
        {            
            var topicFilters = new MqttTopicFilterBuilder()
                .WithTopic(GetTopic(subtopic))
                .Build();
            await client.SubscribeAsync(topicFilters);
        }

        public async Task Publish(string subtopic, string payload)
        {            
            if (client == null)
            {
                Console.WriteLine("Client is null.");
                return;
            }

            Console.WriteLine("Publish: " + payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(GetTopic(subtopic))
                .WithPayload(payload)
                .Build();
            
            await client.PublishAsync(message);
        }        

        private string GetTopic(string subtopic)
        {
            return (topic + "/" + subtopic);
        }

        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = MessageReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public class MessageReceivedEventArgs : EventArgs
        {
            public string Topic { get; set; }

            public string Payload { get; set; }
        }

    }
}
