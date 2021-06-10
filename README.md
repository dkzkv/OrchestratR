# OrchestratR
Orchestrator is a framework that allows you to easily and quickly scale infinite Jobs, supports both horizontal and vertical scaling, providing high reliability and fault tolerance (even after total blackout).

The main feature is the reallocation of Jobs. If a server running 1-N Jobs crashes, these Jobs will be immediately reallocated to free servers that can take them to work. Low Coupling of the system components ensures that the fall of one module will not affect the operation of the others.

Logically, the framework can be divided into two components:

**OrchestratR.Server**
>Orchestration servers are the place where your Jobs will be performed, they can be located on a single machine, or on hundreds of thousands. The framework will make sure that your Job runs on only one server

**OrchestratR.ServerManager**
>The service that manages jobs, is responsible for creating and canceling jobs, and provides tools for tracking the system status and task completion status.

## How to use it all
-------------
In order to start the job we just need to add one line to the `HostBuilder`.
```csharp
services.AddOrchestratedServer(new OrchestratedServerOptions (string serverName, int maxWokrersCount))
```
In OrchestratedServerOptions we should set name of server `serverName`, it doesn't have to be unique. And `MaxWorkersCount` - how many Jobs can be served on this server at the same time.
```csharp
var hostBuilder = new HostBuilder()
    .ConfigureServices((_, services) =>
    {
        var orchestratorOptions = new OrchestratedServerOptions("test-server", 10);
        services.AddOrchestratedServer(orchestratorOptions, async (jobArg, token, heartBeat) =>
        {
            //Place your infinite job hear
            await YourInfiniteJob(jobArg,token, heartBeat);
            
        }).UseRabbitMqTransport(new RabbitMqOptions("localhost","guest","guest"));
    })
    .UseConsoleLifetime();
```
As input parameters for managing our job, the manager sends us:
- `jobArg.JobName` the name of the job, which is unique for the entire cluster
- `jobArg.Argument` any argument for our job. passed as a string, but nothing prevents serializing / deserializing to any object that you need.
- `token` called when the manager gives a task to cancel Job. we need to build the logic of the job so that if the token is called, the work will be completed.
- `heartBeat` it is not necessary to use it, but sometimes it is useful to track the state of work via heartBeat.
Too frequent use can lead to unnecessary load on the system, so use it wisely.

Just an example of some Job and no more:
```csharp
async Task YourInfiniteJob(JobArgument jobArg,CancellationToken token, Func<Task> heartbeat)
{
    Console.WriteLine($"Test job with name: {jobArg.JobName} started.")
    while (!token.IsCancellationRequested) 
    {
        //Some work
        await Task.Delay(1000)
        //Invoke чтобы уведомить менеджера о корректной работе  
        await heartbeat.Invoke();
    }
    Console.WriteLine($"Test job with name: {jobArg.JobName} canceled by manager.")
}
```
-------------
And now about how to manage all this (all our jobs). In the project, you just need to add:
```csharp
services.AddOrchestratedServerManager()
    .UseSqlServerStorage("Server=localhost,1433;Database=....") //standart conncetion string.
    .UseRabbitMqTransport(new RabbitMqOptions("localhost","guest","guest"));
``` 
ServerManager saves to persistence and supports the following databases: **SqlServer**, **PostgreSql**, **MySql** and also **InMemory** storage, which is not recommended. 

Now you can use `IOrchestratorClient` to create and cancel remote jobs:
```csharp
IOrchestratorClient _orchestratorClient; //Injected
...
//This wil run job on one of server
var jobId = await _orchestratorClient.CreateJob("unique_job_name","any_argument", token);

await _orchestratorClient.MarkOnDeleting(jobId, token);
```   
And `IOrchestratorMonitor`/`IAdminOrchestratorMonitor` to observer system, active servers, jobs and their statuses etc.
```csharp
IOrchestratorMonitor _orchestratorMonitor; //Injected
...
var servers = _orchestratorMonitor.Servers(new ServerFilter(),token);

var jobs = _orchestratorMonitor.OrchestratedJobs(new ServerFilter(),token);

var job = _orchestratorMonitor.OrchestratedJob(id,token);
```
There is only one difference between `IOrchestratorMonitor` and `IAdminOrchestratorMonitor`. `IOrchestratorMonitor` returns paged result. And thats all. It may be useful in case when we have millions of jobs and query to get all off them may be too heavy.

As an example, information about the server contains data about what Jobs are currently running on it
```json
{
    "id": "2c321c5c-def2-4b7c-99df-43c7e4486df2",
    "name": "server-1",
    "maxWorkersCount": 10,
    "createdAt": "2021-01-01T01:01:01+00:00",
    "modifyAt": "2021-02-01T01:01:01.+00:00",
    "orchestratedJobs": [
        {
            "id": "32f6c7be-c9c1-4240-b7dd-472b93c8ed1d",
            "name": "test_job_1",
            "status": "Processing"
        },
        {
            "id": "32f6c7be-c9c1-4240-b7dd-472b93c8ed1d",
            "name": "test_job_2",
            "status": "Processing"
        }
    ],
    "isDeleted": false
}
```

### Under the hood
-------------
For the orchestration of Jobs between servers, the Message broker is used (currently only RabbitMQ is implemented). Jobs as a messages are placed in a single queue and distributed among the servers. At the same time, message consumers do not return ACK, just fetch them. This results in an automatic reallocation of tasks in the event of a crash
>This is how jobs distrbuted between servers

![](https://github.com/dkzkv/OrchestratR/blob/main/assets/images/normal-case.png?raw=true)

>And what happens when one server crashes

![](https://github.com/dkzkv/OrchestratR/blob/main/assets/images/error-case.png?raw=true)

**Pros:**
- If one or more servers are disabled, Jobs will be distributed evenly among the active ones
- Distribution occurs without delays, polling, and so on
- The manager does not affect the work of Jobs and their distribution between servers, and is only responsible for creating and canceling them
- The manager automatically comes to a consistent state when turned on and contains up-to-date information on the operation of servers and Jobs
- After a complete blackout, the system is automatically restored to a working state

**Cons:**
- The system depends on the Message broker. If the Message broker is unavailable, Jobs stop running until the broker is available. This will redistribute Jobs across the servers. When designing the application architecture, we need to make sure that brocker is always available, they are easily clustered, which solves this problem.
- Broker delivery semantics do not always guarantee a single message delivery. RabbitMQ supports At-least once delivery, which in rare cases will lead to the fact that the task that we sent for execution can be duplicated and executed at the moment on different servers. [This plugin](https://github.com/noxdafox/rabbitmq-message-deduplication) removes duplicates from the queue, which solves this problem