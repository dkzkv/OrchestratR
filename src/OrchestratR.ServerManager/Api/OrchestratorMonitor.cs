using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Paging;
using OrchestratR.ServerManager.Domain.Queries;
using OrchestratR.ServerManager.Domain.Queries.QueryModels;

namespace OrchestratR.ServerManager.Api
{
    internal class OrchestratorMonitor : BaseOrchestratorClient, IOrchestratorMonitor, IAdminOrchestratorMonitor
    {
        public OrchestratorMonitor([NotNull] IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        async Task<Page<IServer>> IOrchestratorMonitor.Servers(ServerFilter filter, CancellationToken token)
        {
            return await ScopedMediator(async (mediator)
                => await mediator.Send(new ServerQuery(filter), token));
        }
        
        async Task<Page<IOrchestratedJob>> IOrchestratorMonitor.OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token)
        {
            return await ScopedMediator(async (mediator)
                => await mediator.Send(new OrchestratedJobQuery(filter), token));
        }

        async Task<IEnumerable<IOrchestratedJob>> IAdminOrchestratorMonitor.OrchestratedJobs(OrchestratedJobFilter filter, CancellationToken token)
        {
            return await ScopedMediator(async (mediator)
                => await mediator.Send(new AdminOrchestratedJobQuery(filter), token));
        }

        async Task<IEnumerable<IServer>> IAdminOrchestratorMonitor.Servers(ServerFilter filter, CancellationToken token)
        {
            return await ScopedMediator(async (mediator)
                => await mediator.Send(new AdminServerQuery(filter), token));
        }
        
        public async Task<IOrchestratedJob> OrchestratedJob(Guid id, CancellationToken token = default)
        {
            return await ScopedMediator(async (mediator)
                => await mediator.Send(new OrchestratedJobByIdQuery(id), token));
        }
    }
}