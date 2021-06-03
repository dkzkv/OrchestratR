using System;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Messages;
using OrchestratR.Server.Common;
using OrchestratR.Server.Consumers;
using OrchestratR.Server.Options;

namespace OrchestratR.Server
{
    public static class OrchestratorServerExtension
    {
        public static IServerTransportConfigurator AddOrchestratedServer(this IServiceCollection services, OrchestratedServerOptions serverOptions,
            Func<string,string,CancellationToken, Task> jobAction)
        {
            IServiceCollectionBusConfigurator serviceCollectionBusConfigurator = null;
            services.AddTransient(_ => new OrchestratedJob(jobAction));
            services.AddSingleton<JobManager>();
            services.AddSingleton(serverOptions);
            
            services.AddMassTransit(x =>
            {
                serviceCollectionBusConfigurator = x;
                x.AddConsumer<JobConsumer>();
                x.AddConsumer<JobCancellationConsumer>();
                MessageCorrelation.UseCorrelationId<IStartJobMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<ICancelJobMessage>(o => o.CorrelationId);
            });
            services.AddHostedService<OrchestratorService>();
            return new ServerTransportConfigurator(serverOptions.OrchestratorName,serverOptions.MaxWorkersCount, serviceCollectionBusConfigurator);
        }
    }
}