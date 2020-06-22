# netFIELD direct Methods MQTT publisher example

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

This example code shows how to create a Edge based application which publishes sample data to a MQTT Bus and how to utilize Microsoft direct Methods to communicate through an API to this container to change values.

This method will register the direct methods on the the EdgeHub.
Use the api.netfield.io API POST /devices/{deviceId}/methods in this way
In this sample the parameter "input1" is used to set the target temperature
```
{
  "containerName": "netfield-app-sample-multi-arch",
  "methodName": "SetTargetTemperature",
  "methodPayload": {
    "temperature": "180",
    "unit": "centigrade"
  }
}
```
## Build and publish your Docker image
adjust your dockerfile accordingly
 
Login to Docker
```sudo docker login```

### arm
Build your docker image
```docker build -t 'your-registry-uri/your-registry':netfied-device-sample-arm32-0.3.0```

Push your docker image to your registry
```docker push 'your-registry-uri/your-registry':netfied-device-sample-arm32-0.3.0```

### x86
Build your docker image
```docker build -t 'your-registry-uri/your-registry':netfied-device-sample-x86-0.3.0```

Push your docker image to your registry (arm)
```docker push 'your-registry-uri/your-registry':netfied-device-sample-x86-0.3.0```

### multi-arch
```
vi ~/.docker/config.json
add option: "experimental": "enabled"

docker manifest create 'your-registry-uri/your-registry':netfield-app-sample-multi-arch 'your-registry-uri/your-registry':netfield-app-sample-x86-0.3.0 'your-registry-uri/your-registry':netfield-app-sample-arm32-0.2.0 --amend

docker manifest annotate 'your-registry-uri/your-registry':netfield-app-sample-multi-arch 'your-registry-uri/your-registry':netfield-app-sample-arm32-0.3.0 --os linux --arch arm

docker manifest push 'your-registry-uri/your-registry':netfield-app-sample-multi-arch --purge
```

You my use the file 'node-red_direct-method_SetTargetTemperature_sample WO credentials.txt' in a Node-Red to test your installation.
