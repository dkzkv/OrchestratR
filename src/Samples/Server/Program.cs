using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OrchestratR.Server;
using OrchestratR.Server.Options;
using OrchestratR.Extension.RabbitMq;
using OrchestratR.Extension.RabbitMq.Options;
using Serilog;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var serverName = "test-orc";
            var maxWorkersCount = 10;
            
            Log.Information("Services started.");
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var orchestratorOptions = new OrchestratedServerOptions(serverName, maxWorkersCount);
                    services.AddOrchestratedServer(orchestratorOptions, async (jobArg, token, heartBeat) =>
                    {
                        
                        Log.Logger.Information($"Job started with name: {jobArg.Name}, argument {jobArg.Argument}");
                        
                        await YourInfiniteJob(jobArg,token, heartBeat,Log.Logger);
                        
                        Log.Logger.Information($"Job: {jobArg.Name} cancel requested.");
                        
                    }).UseRabbitMqTransport(new RabbitMqOptions("localhost", "guest", "guest"));
                })
                .UseSerilog()
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }

        static async Task YourInfiniteJob(JobArgument jobArg,CancellationToken token, Func<Task> heartbeat, ILogger logger)
        {
            int i = 0;
            while (!token.IsCancellationRequested)
            {
                logger.Information($"Job: {jobArg.Name}, incremented: {++i}");
                await Task.Delay(1000,token);
                await heartbeat.Invoke();
            }
        }
    }
}