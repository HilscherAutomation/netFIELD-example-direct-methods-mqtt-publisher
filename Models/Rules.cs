using System.Collections.Generic;

namespace NetfieldDeviceSample.Models
{
    public class Rules
    {
        public List<Data> TargetData {get; private set;}
        public int MaxTemperature {get; private set;}

        public Rules() // create rule table and set max. temperature
        {
           TargetData = new List<Data>();
           int Temperature = 180;
           int Speed = 1;

           while(Speed <= 10)
           {
               Data data = new Data();
               data.DataType = "target";
               data.Temperature = Temperature;
               data.Speed = Speed;
               TargetData.Add(data);
               MaxTemperature = Temperature;
               Temperature = Temperature + 4;
               Speed = Speed+1;
           }
        }
    }
}