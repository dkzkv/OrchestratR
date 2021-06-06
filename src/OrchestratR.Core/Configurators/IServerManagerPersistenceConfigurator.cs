using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.Core.Configurators
{
    public interface IServerManagerPersistenceConfigurator
    {
        IServiceCollection Services { get; }
        IServerManagerTransportConfigurator TransportConfigurator { get; }
    }
}