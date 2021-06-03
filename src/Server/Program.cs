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
                    services.AddOrchestratedServer(orchestratorOptions, async (name, argument, token) =>
                    {
                        await SomeLongRunningJob(name,argument,token ,Log.Logger);
                    }).UseRabbitMqTransport(new RabbitMqOptions("localhost", "guest", "guest"));
                })
                .UseSerilog()
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }

        static async Task SomeLongRunningJob(string name,string argument,CancellationToken token, ILogger logger)
        {
            logger.Information($"Job started with name: {name}, argument {argument}");
            int i = 0;
            while (!token.IsCancellationRequested)
            {
                
                logger.Information($"Job: {name}, incremented: {++i}");
                await Task.Delay(1000,token);
            }
            logger.Information($"Job: {name} cancel requested.");
        }
    }
}