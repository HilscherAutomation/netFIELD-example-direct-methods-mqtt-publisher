namespace NetfieldDeviceSample.Models
{
    public class DirectMethodParam
    {
        public Payload payload {get; set;}
    }

    public class Payload
    {
        public string input1 {get; set;} // target temperature
        public string input2 {get; set;} // unit
    }
}