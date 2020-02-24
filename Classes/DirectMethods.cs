using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using NetfieldDeviceSample.Models;

namespace NetfieldDeviceSample.Classes
{
    public class DirectMethods
    {
        public static ModuleClient ioTHubModuleClient { get; private set; }
        private static SimulateData _simulateData {get; set;}

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
        "containerName": "netfield-app-sample-arm32",
        "methodName": "SetTargetTemperature",
        "methodPayload": {
            "payload": {
            "input1": "180",
            "input2": "centigrade"
            }
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
            Console.WriteLine("SetTargetTemperature has been called");
            Console.WriteLine($"Method Payload: {methodRequest.DataAsJson}");

            DirectMethodParam param = new DirectMethodParam();
            param = JsonSerializer.Deserialize<DirectMethodParam>(methodRequest.DataAsJson);

            _simulateData.SetTemperature(int.Parse(param.payload.input1));
        
            return new MethodResponse(200);
        }
    }
}