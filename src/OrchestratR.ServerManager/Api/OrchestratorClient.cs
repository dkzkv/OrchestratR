using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OrchestratR.ServerManager.Domain.Commands;

namespace OrchestratR.ServerManager.Api
{
    internal class OrchestratorClient :BaseOrchestratorClient, IOrchestratorClient
    {
        public OrchestratorClient([NotNull] IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public async  Task<Guid> CreateJob(string name, string argument, CancellationToken token = default)
        {
            return await ScopedMediator(async (mediator) 
                => await mediator.Send(new CreateJobCommand(name, argument), token));
        }
        
        public async Task MarkOnDeleting(Guid id, CancellationToken token = default)
        {
            await ScopedMediator(async (mediator) 
                => await mediator.Send(new MarkAsDeletedJobCommand(id), token));
        }
    }
}