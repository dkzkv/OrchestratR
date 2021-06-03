using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchestratR.ServerManager.Configurators;
using OrchestratR.ServerManager.Domain.Interfaces;
using OrchestratR.ServerManager.Persistence.MsSql.Migrations;
using OrchestratR.ServerManager.Persistence.Repositories;

namespace OrchestratR.ServerManager.Persistence.MsSql
{
    public static class ServerManagerPersistenceExtension
    {
        public static IServerManagerTransportConfigurator UseSqlServerStorage(this IServerManagerPersistenceConfigurator configurator,
            string connectionString)
        {
            configurator.Services.AddDbContext<OrchestratorDbContext>(
                (provider, builder) => builder.UseSqlServer(connectionString, builder =>
                {
                    builder.MigrationsAssembly("OrchestratR.ServerManager.Persistence.MsSql");
                    builder.MigrationsHistoryTable("__MyMigrationsHistory", "orchestrator");
                }));
            
            return configurator.TransportConfigurator;
        }
    }
}