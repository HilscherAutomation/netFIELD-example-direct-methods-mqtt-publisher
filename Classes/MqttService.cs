using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;

namespace NetfieldDeviceSample.Classes
{
    public class MqttService
    {
        public IManagedMqttClient mqttClient { private set; get; }

        public MqttService(string mqttbroker, int port)
        {
            string uuid = Guid.NewGuid().ToString();
            
            var options = new MqttClientOptionsBuilder()
                        .WithClientId(uuid)
                        .WithTcpServer(mqttbroker, port)
                        //.WithCredentials("bud", "%spencer%")
                        //.WithTls()
                        .WithCleanSession(true)
                        .Build();

            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                        .WithClientOptions(options)
                        .Build();

            mqttClient = new MqttFactory().CreateManagedMqttClient();
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MessageReceiveHandler);
            mqttClient.StartAsync(managedMqttClientOptions);
        }

        // messages received from subscribed topic
        public void MessageReceiveHandler(MqttApplicationMessageReceivedEventArgs e)
        {
            Console.WriteLine($"Message :{e.ApplicationMessage.Payload.ToString()} from Topic :{e.ApplicationMessage.Topic} received");
        }

        // Publish a message to a specific topic
        public async Task PublishAsync(string topic, string payload, int qos = 0, bool retainflag = false)
        {
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
              .WithTopic(topic)
              .WithPayload(payload)
              .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
              .WithRetainFlag(retainflag)
              .Build());
            Console.WriteLine($"Write to Topic:{topic}");
        }

        // subscribe to a specific topic
        public async Task SubscribeTopicAsync(string topic, int qos = 0)
        {
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
              .WithTopic(topic)
              .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
              .Build());
        }

    }
}