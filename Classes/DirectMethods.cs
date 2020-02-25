using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using NetfieldDeviceSample.Models;

namespace NetfieldDeviceSample.Classes
{
    public class DirectMethods
    {
        public static ModuleClient ioTHubModuleClient { get; private set; }
        private static SimulateData _simulateData { get; set; }

        /// <summary>
        /// Initializes the ModuleClient
        /// </summary>
        public static async Task Init(SimulateData simulateData)
        {
            AmqpTransportSettings amqpSetting = new AmqpTransportSettings(TransportType.Amqp_Tcp_Only);
            ITransportSettings[] settings = { amqpSetting };
            _simulateData = simulateData;

            // Open a connection to the Edge runtime
            ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            await RegisterDirectMethodsAsync();
        }

        /// <summary>
        /// This method will register the direct methods on the the EdgeHub.
        /// Use the api.netfield.io API POST /devices/{deviceId}/methods in this way
        /// In this sample the parameter "input1" is used to set the target temperature
        /*
          {
            "containerName": "<container name>",
            "methodName": "SetTargetTemperature",
            "methodPayload": {
                "temperature": "180",
                "unit": "centigrade"
            }
          }
        */
        /// </summary>
        
        public static async Task RegisterDirectMethodsAsync()
        {
            Console.WriteLine("Registering direct method callbacks");
            await ioTHubModuleClient.SetMethodHandlerAsync("SetTargetTemperature", OnSetTargetTemperature, null);
        }

        private static async Task<MethodResponse> OnSetTargetTemperature(MethodRequest methodRequest, object userContext)
        {
            DirectMethodResult result = new DirectMethodResult();

            Console.WriteLine("SetTargetTemperature has been called");
            Console.WriteLine($"Method Payload: {methodRequest.DataAsJson}");

            MethodPayload methodPayload = new MethodPayload();
            methodPayload = JsonSerializer.Deserialize<MethodPayload>(methodRequest.DataAsJson);

            int TargetTemperature = int.Parse(methodPayload.temperature);

            if (TargetTemperature >= _simulateData.RuleList.MinTemperature && TargetTemperature <= _simulateData.RuleList.MaxTemperature)
            {
                _simulateData.SetTemperature(TargetTemperature);
                result.methodPayload = methodPayload;
                result.result = true;
                result.msg = "ok";
            }
            else
            {
                result.methodPayload = methodPayload;
                result.result = false;
                result.msg = $"Target temperature out of range: {_simulateData.RuleList.MinTemperature} ... {_simulateData.RuleList.MaxTemperature}";

            }

            byte[] ResultAsJson = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(result));
            MethodResponse mr = new MethodResponse(ResultAsJson, 200);

            return mr;
        }
    }
}