# SmartIoTPlatform
[Before]
* SmartIoTPlatform is a new project which I designed SmartIoTPlatform for hug, distribute data process system. Be aware that I just developed this platform and many modules are not fully ready. If you are looking for a suitable system to collect data from equipment, you are welcome to use source code and contribute your knowledge in this platform. I hope that this platform can help manufacture and become a standard in the future.

[Reason to use SmartIoTPlatform]
* SmartIoTPlatform designed for high speed/reliable data exchange mechanism using MSMQ. You can create services to process IoT collect data. And data is easily to be accessed by service. SmartIoTPlatform provide the best platform to integrate data and service for IoT platform.

[Best practice in manufacture]
* SmartIoTPlatform is suitable to 365-24 equipment operation. The reason is after equipment's data send to SmartIoTPlatform, data can be transferred, duplicated, merged in this platform. It won't impact the equipment data collect, so it reduce the risk if data processing is changed in production line.

[Rich data type support]
    Most of PLCs use tag data, the data type is limited with some primary type. MQ allow you to transfer raw data, image or files. So it create many possibility:
1. For manufacture dashboard, SmartIoTPlatform allow you to collect raw data and publish the data to dashbard and host like MES or other IoT platform in the same time. So if some alarms happen, you can deliver the alarm ID with instruction like picture or movies. It help manufacture to create "smart" advance system to handle problem.
2. Some fast millisecond data occur, SmartIoTPlatform can deploy the data to and handle by many system. That means you can separate difference service to handle the same data and reduce central system loading.
3. SmartIoTPlatform will design routing manager to route data to other queues. Data can be transfer to "security" queue and EAP won't know the internal data process. It helps to improve data processing security for industry.

[Data exchange layer]
* The data exchange layer is Microsoft message queue (MSMQ). It is allow to change to other message queue like RabbitMQ, ActiveMQ etc. but not implement yet and need your contribution.
* Some reason to use MSMQ in this moment that the queue security can manage with AD. The system provide some queue read/write protection and it reduce your effort to manage the queues user profile. Some hug organization and a lot of EAP(equipment access program) computers. For manager view, some loading can balance with IT role.
* If you have concern to develop services with multi-OS (like linux) or multi-language like Java, Python, GoLang..., I will suggest to change the MSMQ to other MQ. But in this moment, I am sorry that due to my job, I don't have too much resource to do it now.
* Be aware that each MSMQ packet has 4MB limitation. RabbitMQ packet size limitation is 128MB. Before you select MQ system, please check the MQ limitation.


Miracle Shih
miracle.shih@gmail.com

