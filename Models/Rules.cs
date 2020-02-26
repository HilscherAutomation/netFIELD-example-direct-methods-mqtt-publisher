using System.Collections.Generic;

namespace NetfieldDeviceSample.Models
{
    public class Rules
    {
        public List<MqttPubData> TargetData { get; private set; }
        public int MaxTemperature { get; private set; }
        public int MinTemperature { get; private set; } = 180;

        public Rules() // create rule table and set max. temperature
        {
            TargetData = new List<MqttPubData>();
            int Temperature = MinTemperature;
            int Speed = 1;

            while (Speed <= 10)
            {
                MqttPubData mqttPubData = new MqttPubData();
                mqttPubData.DataType = "target";
                mqttPubData.Temperature = Temperature;
                mqttPubData.Speed = Speed;
                TargetData.Add(mqttPubData);
                MaxTemperature = Temperature;

                int counter = 1;
                while (counter < 4 && Temperature < 216)
                {
                    mqttPubData = new MqttPubData();
                    mqttPubData.DataType = "target";
                    mqttPubData.Temperature = Temperature + 1;
                    mqttPubData.Speed = Speed;
                    TargetData.Add(mqttPubData);
                    Temperature = Temperature + 1;
                    counter = counter + 1;
                }

                Temperature = Temperature + 1;
                Speed = Speed + 1;
            }
        }
    }
}