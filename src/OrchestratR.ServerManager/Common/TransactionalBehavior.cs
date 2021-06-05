using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrchestratR.ServerManager.Domain.Commands;
using OrchestratR.ServerManager.Persistence;

namespace OrchestratR.ServerManager.Common
{
    public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly OrchestratorDbContext _orchestratorDbContext;

        public TransactionalBehavior(OrchestratorDbContext orchestratorDbContext)
        {
            _orchestratorDbContext = orchestratorDbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (request is not ITransactionalCommand)
            {
                return await next();
            }

            using var transaction = await _orchestratorDbContext.Database.BeginTransactionAsync(cancellationToken);
            
            var response = await next();
            
            await _orchestratorDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return response;
        }
    }
}