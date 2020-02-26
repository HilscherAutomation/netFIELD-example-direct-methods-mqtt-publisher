using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetfieldDeviceSample.Classes;
using NetfieldDeviceSample.Models;

namespace NetfieldDeviceSample
{
    public class Application : IHostedService, IDisposable
    {
        private readonly IOptions<MqttConfig> _mqttconfig;
        private MqttService mqttService {get; set;}
        public SimulateData simulateData {get; private set;}

        public Application(IOptions<MqttConfig> mqttConfig)
        {
            _mqttconfig = mqttConfig;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Connect to MQTT Broker");
            mqttService = new MqttService(_mqttconfig.Value.Server, _mqttconfig.Value.Port);
            simulateData = new SimulateData(mqttService);
            await DirectMethods.Init(simulateData);
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}