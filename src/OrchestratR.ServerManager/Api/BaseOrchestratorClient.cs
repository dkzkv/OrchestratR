using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.ServerManager.Api
{
    internal abstract class BaseOrchestratorClient
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected BaseOrchestratorClient([NotNull] IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected async Task<T> ScopedMediator<T>(Func<IMediator,Task<T>> func)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            if (mediator is null)
                throw new InvalidOperationException("Mediator was not resolved for orchestrated manager client.");

            return await func.Invoke(mediator);
        }
    }
}