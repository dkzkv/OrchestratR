using System;
using MassTransit;
using MassTransit.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Messages;
using OrchestratR.ServerManager.Configurators;
using OrchestratR.ServerManager.Consumers;
using OrchestratR.ServerManager.Domain;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Persistence.Repositories;

namespace OrchestratR.ServerManager
{
    public static class OrchestratorServerManagerExtension
    {
        private static string PersistenceAssembly => "OrchestratR.ServerManager.Persistence";
        public static IServerManagerPersistenceConfigurator AddOrchestratedServerManager(this IServiceCollection services)
        {
            services.AddTransient<ServerService>();
            services.AddTransient<JobOrchestrationService>();
            services.AddTransient<IJobOrchestrationClient, JobOrchestrationService>();
            services.AddMediatR(AppDomain.CurrentDomain.Load(PersistenceAssembly));
            services.AddAutoMapper(AppDomain.CurrentDomain.Load(PersistenceAssembly) );
            services.AddTransient<IJobRepository, JobRepository>();
            services.AddTransient<IServerRepository, ServerRepository>();
            
            services.AddHostedService<OrchestratorManagerService>();
            ServerManagerTransportConfigurator transportConfigurator = null;
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ServerConsumer>();
                x.AddConsumer<JobStatusConsumer>();

                MessageCorrelation.UseCorrelationId<IServerCreatedMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IServerDeletedMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IJobStatusMessage>(o => o.CorrelationId);
                transportConfigurator = new ServerManagerTransportConfigurator(x);
            });
            return new ServerManagerPersistenceConfigurator(services, transportConfigurator);
        }
    }
}