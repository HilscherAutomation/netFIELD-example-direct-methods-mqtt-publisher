namespace NetfieldDeviceSample.Models
{
    public class MqttPubData
    {
        public string DataType {get; set;} // shall be "actual" or "target"
        public int Temperature {get; set;}
        public int Speed {get; set;}
    }
}