using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Context;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;
using OrchestratR.Core.Messages;
using OrchestratR.Core.Publishers;
using OrchestratR.Server.Common;
using OrchestratR.Server.Configurators;
using OrchestratR.Server.Consumers;
using OrchestratR.Server.Messaging;
using OrchestratR.Server.Options;

namespace OrchestratR.Server
{
    public class JobArgument
    {
        public JobArgument(string name, string argument)
        {
            Name = name;
            Argument = argument;
        }
        
        public string Name { get; }
        public string Argument { get; }
    }
    
    public static class OrchestratorServerExtension
    {
        public static IServerTransportConfigurator AddOrchestratedServer(this IServiceCollection services, OrchestratedServerOptions serverOptions,
            Func<JobArgument,CancellationToken,Func<Task>,IServiceProvider, Task> jobAction)
        {
            services.AddTransient(_ => new OrchestratedJob(jobAction));
            return BaseConfiguration(services, serverOptions);
        }
        
        public static IServerTransportConfigurator AddOrchestratedServer(this IServiceCollection services, OrchestratedServerOptions serverOptions,
            Func<JobArgument,CancellationToken,Func<Task>, Task> jobAction)
        {
            services.AddTransient(_ => new OrchestratedJob(jobAction));
            return BaseConfiguration(services, serverOptions);
        }
        
        private static IServerTransportConfigurator BaseConfiguration(IServiceCollection services, OrchestratedServerOptions serverOptions)
        {
            IServiceCollectionBusConfigurator serviceCollectionBusConfigurator = null;
            services.AddSingleton<JobManager>();
            services.AddSingleton(serverOptions);
            services.AddSingleton<IServerPublisher,ServerPublisher>();
            
            services.AddMassTransit(x =>
            {
                serviceCollectionBusConfigurator = x;
                x.AddConsumer<JobConsumer>();
                x.AddConsumer<JobCancellationConsumer>();
                MessageCorrelation.UseCorrelationId<IStartJobMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IStopJobMessage>(o => o.CorrelationId);
            });
            services.AddHostedService<OrchestratorService>();
            return new ServerTransportConfigurator(serverOptions.OrchestratorName,serverOptions.MaxWorkersCount, serviceCollectionBusConfigurator);
        } 
    }
}