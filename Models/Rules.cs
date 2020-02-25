using System.Collections.Generic;

namespace NetfieldDeviceSample.Models
{
    public class Rules
    {
        public List<Data> TargetData { get; private set; }
        public int MaxTemperature { get; private set; }
        public int MinTemperature { get; private set; } = 180;

        public Rules() // create rule table and set max. temperature
        {
            TargetData = new List<Data>();
            int Temperature = MinTemperature;
            int Speed = 1;

            while (Speed <= 10)
            {
                Data data = new Data();
                data.DataType = "target";
                data.Temperature = Temperature;
                data.Speed = Speed;
                TargetData.Add(data);
                MaxTemperature = Temperature;

                int counter = 1;
                while (counter < 4 && Temperature < 216)
                {
                    data = new Data();
                    data.DataType = "target";
                    data.Temperature = Temperature + 1;
                    data.Speed = Speed;
                    TargetData.Add(data);
                    Temperature = Temperature + 1;
                    counter = counter + 1;
                }

                Temperature = Temperature + 1;
                Speed = Speed + 1;
            }
        }
    }
}