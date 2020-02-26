namespace NetfieldDeviceSample.Models
{
    public class MethodPayload
    {
        public string temperature {get; set;} // target temperature
        public string unit {get; set;} // unit
    }

    public class DirectMethodResult
    {
        public MethodPayload methodPayload {get; set;}
        public bool successful  {get; set;}
        public string msg {get; set;}
    }
}