using System;
using System.Timers;
using System.Text.Json;
using NetfieldDeviceSample.Models;

namespace NetfieldDeviceSample.Classes
{
    public class SimulateData
    {
        private readonly MqttService _mqttService;
        private Timer dataTimer { set; get; }
        private int TimerInSeconds = 5;
        public Rules RuleList { get; private set; }
        private int Temperature { get; set; }

        public SimulateData(MqttService mqttService)
        {
            _mqttService = mqttService;

            // create temperature / speed relation rules
            RuleList = new Rules();

            // set start temperature
            SetTemperature(180);

            // start timer for publishing the simulated test data frequently
            dataTimer = new Timer(TimerInSeconds * 1000);
            dataTimer.Elapsed += OnTimer;
            dataTimer.AutoReset = true;
            dataTimer.Enabled = true;
        }

        private async void OnTimer(Object source, ElapsedEventArgs e)
        {
            MqttPubData mqttPubData = new MqttPubData();

            foreach (MqttPubData d in RuleList.TargetData)
            {
                if (Temperature == d.Temperature)
                {
                    mqttPubData.DataType = "actual";
                    mqttPubData.Temperature = Temperature;
                    mqttPubData.Speed = d.Speed;
                }
            }

            string json = JsonSerializer.Serialize(mqttPubData);

            // increase temperature automatically
            if (Temperature == RuleList.MaxTemperature)
                SetTemperature(RuleList.MinTemperature);
            else
                Temperature = Temperature + 1;

            await _mqttService.PublishAsync("processdata", json);
        }

        public void SetTemperature(int temperature)
        {
            Temperature = temperature;

            Console.WriteLine($"Target Temperatur set to: {temperature.ToString()}");
        }
    }
}