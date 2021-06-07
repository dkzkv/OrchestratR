using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OrchestratR.Server;
using OrchestratR.Server.Options;
using OrchestratR.Extension.RabbitMq;
using OrchestratR.Extension.RabbitMq.Options;
using Serilog;
using Server.Options;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();
            
            var serverOptions = configuration.GetSection("ServerOptions").Get<ServerOptions>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .CreateLogger()
                .ForContext("OrchestratorType", "Server")
                .ForContext("OrchestratorServer", serverOptions.ServerName);

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((_, configApp) => { configApp.AddConfiguration(configuration); })
                .ConfigureServices((_, services) =>
                {
                    var orchestratorOptions = new OrchestratedServerOptions(serverOptions.ServerName, serverOptions.MaxWorkersCount);
                    services.AddOrchestratedServer(orchestratorOptions, async (jobArg, token, heartBeat) =>
                    {

                        await YourInfiniteJob(jobArg,token, heartBeat, Log.Logger);
                        
                    }).UseRabbitMqTransport(
                        new RabbitMqOptions(serverOptions.RabbitMq.Host, serverOptions.RabbitMq.UserName, serverOptions.RabbitMq.Password));
                })
                .UseSerilog()
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }

        static async Task YourInfiniteJob(JobArgument jobArg,CancellationToken token, Func<Task> heartbeat, ILogger logger)
        {
            Log.Logger.Information($"Job started with name: {jobArg.Name}, argument {jobArg.Argument}");
            
            int i = 0;
            while (!token.IsCancellationRequested)
            {
                logger.Information($"Job: {jobArg.Name}, incremented: {++i}");
                await Task.Delay(5000,token);
                await heartbeat.Invoke();
            }
            
            Log.Logger.Information($"Job: {jobArg.Name} cancel requested.");
        }
    }
}