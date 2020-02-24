#netFIELD direct Methods MQTT publisher example

This example code shows how to create a Edge based application which publishes sample data to a MQTT Bus and how to utilize Microsoft direct Methods to communicate through an API to this container to change values.

This method will register the direct methods on the the EdgeHub.
Use the api.netfield.io API POST /devices/{deviceId}/methods in this way
In this sample the parameter "input1" is used to set the target temperature
```
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
```

adjust your dockerfile accordingly

Login to Docker
```sudo docker login```

Build your docker image
```docker build -t 'your-registry-uri/your-registry':netfied-device-sample-arm32-0.1.0```

Push your docker image to your registry
```docker push 'your-registry-uri/your-registry':netfied-device-sample-arm32-0.1.0```
