using System;
using GreenPipes;
using MassTransit;
using OrchestratR.Extension.RabbitMq.Options;
using OrchestratR.Server;
using OrchestratR.Server.Common;
using OrchestratR.Server.Consumers;
using OrchestratR.ServerManager.Configurators;
using OrchestratR.ServerManager.Consumers;

namespace OrchestratR.Extension.RabbitMq
{
    public static class OrchestratorServerExtension
    {
        public static void UseRabbitMqTransport(this IServerTransportConfigurator configurator, RabbitMqOptions options)
        {
            if (configurator.BusConfigurator is null)
                throw new ArgumentException("Bus configurator is not implemented.");
            
            configurator.BusConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(options.Host, h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });
                // RoundRobin
                cfg.ReceiveEndpoint(OrchestratorQueueConstants.OrchestratorJobsPrefix + configurator.OrchestratorServerName, e =>
                {
                    e.Durable = true;
                    e.UseMessageRetry(r => r.Immediate(int.MaxValue));
                    e.PrefetchCount = configurator.MaxWorkersCount;
                    e.ConfigureConsumer<JobConsumer>(provider);
                });

                // Fan-out
                cfg.ReceiveEndpoint(OrchestratorQueueConstants.CancellationJobPrefix + configurator.OrchestratorServerName + '_' + Guid.NewGuid(), e =>
                {
                    e.PrefetchCount = 1;
                    e.AutoDelete = true;
                    e.ConfigureConsumer<JobCancellationConsumer>(provider);
                });
            }));
        }
        
        public static void UseRabbitMqTransport(this IServerManagerTransportConfigurator configurator, RabbitMqOptions options)
        {
            if (configurator.BusConfigurator is null)
                throw new ArgumentException("Bus configurator is not implemented.");
            
            configurator.BusConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(options.Host, h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });
                
                cfg.ReceiveEndpoint(OrchestratorQueueConstants.Manager, e =>
                {
                    e.UseMessageRetry(r => r.Interval(10, TimeSpan.FromSeconds(5)));
                    e.PrefetchCount = 1;
                    e.ConfigureConsumer<ServerConsumer>(provider);
                    e.ConfigureConsumer<JobStatusConsumer>(provider);
                });
            }));
        }
    }
}