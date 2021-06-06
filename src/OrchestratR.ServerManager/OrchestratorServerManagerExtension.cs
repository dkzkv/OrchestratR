using System;
using System.Reflection;
using MassTransit;
using MassTransit.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;
using OrchestratR.Core.Messages;
using OrchestratR.Core.Publishers;
using OrchestratR.ServerManager.Api;
using OrchestratR.ServerManager.Common;
using OrchestratR.ServerManager.Configurators;
using OrchestratR.ServerManager.Consumers;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Messaging;
using OrchestratR.ServerManager.Persistence.Repositories;

namespace OrchestratR.ServerManager
{
    public static class OrchestratorServerManagerExtension
    {
        private static string DomainAssembly => "OrchestratR.ServerManager.Domain";
        private static string PersistenceAssembly => "OrchestratR.ServerManager.Persistence";

        public static IServerManagerPersistenceConfigurator AddOrchestratedServerManager(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly(),
                AppDomain.CurrentDomain.Load(DomainAssembly),
                AppDomain.CurrentDomain.Load(PersistenceAssembly));
            
            services.AddAutoMapper(AppDomain.CurrentDomain.Load(PersistenceAssembly));
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IServerRepository, ServerRepository>();
            
            services.AddScoped<IOrchestratorClient, OrchestratorClient>();
            services.AddScoped<IAdminOrchestratorMonitor, OrchestratorMonitor>();
            services.AddScoped<IOrchestratorMonitor, OrchestratorMonitor>();
            services.AddScoped<IServerManagerPublisher, ServerManagerPublisher>();
            
            //pipeline
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));
            
            services.AddSingleton<IOrchestratorManagerService, OrchestratorManagerService>();
            services.AddHostedService<OrchestratorManagerService>();

            ServerManagerTransportConfigurator transportConfigurator = null;
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ServerConsumer>();
                x.AddConsumer<JobStatusConsumer>();
                x.AddConsumer<HeartBeatConsumer>();
                
                MessageCorrelation.UseCorrelationId<IJobHearBeatMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IServerCreatedMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IServerDeletedMessage>(o => o.CorrelationId);
                MessageCorrelation.UseCorrelationId<IJobStatusMessage>(o => o.CorrelationId);
                transportConfigurator = new ServerManagerTransportConfigurator(x);
            });
            return new ServerManagerPersistenceConfigurator(services, transportConfigurator);
        }
    }
}