using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchestratR.Extension.RabbitMq;
using OrchestratR.Extension.RabbitMq.Options;
using OrchestratR.ServerManager;
using OrchestratR.ServerManager.Domain;
using OrchestratR.ServerManager.Persistence.MsSql;
using Serilog;

namespace ServerManager
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
            
            Log.Information("Services started.");
            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOrchestratedServerManager()
                        .UseSqlServerStorage("Server=localhost,1433;Database=TestDb;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=true")
                        .UseRabbitMqTransport(new RabbitMqOptions("localhost", "guest", "guest"));

                    services.AddHostedService<ConsoleJobManagerService>();
                })
                .UseSerilog()
                .UseConsoleLifetime();

            await hostBuilder.RunConsoleAsync();
        }

        internal class ConsoleJobManagerService : IHostedService
        {
            private readonly IServiceProvider _serviceCollection;

            public ConsoleJobManagerService(IServiceProvider serviceCollection)
            {
                _serviceCollection = serviceCollection;
            }

            public async Task StartAsync(CancellationToken token)
            {
                /*Console.WriteLine("Press any key to create job.");
                Console.ReadKey();
                var client1 = _serviceCollection.CreateScope();
                var id = await client1.Create("test", "arg1",token);
                
                Console.WriteLine("Press any key to cancel job.");
                Console.ReadKey();
                var client2 = _serviceCollection.GetService<IJobOrchestrationClient>();
                await client2.MarkOnDeleting(id,token);*/
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}