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
        private Rules RuleList { get; set; }
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
            Data data = new Data();

            foreach (Data d in RuleList.TargetData)
            {
                if (Temperature == d.Temperature)
                {
                    data.DataType = "actual";
                    data.Temperature = Temperature;
                    data.Speed = d.Speed;
                }
            }

            string json = JsonSerializer.Serialize(data);

            // increase temperature automatically
            if (Temperature == RuleList.MaxTemperature)
                SetTemperature(180);
            else
                Temperature = Temperature + 4;

            await _mqttService.PublishAsync("processdata", json);
        }

        public void SetTemperature(int temperature)
        {
            Temperature = temperature;

            Console.WriteLine($"Target Temperatur set to: {temperature.ToString()}");
        }
    }
}