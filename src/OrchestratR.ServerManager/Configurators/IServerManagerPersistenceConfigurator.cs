using Microsoft.Extensions.DependencyInjection;

namespace OrchestratR.ServerManager.Configurators
{
    public interface IServerManagerPersistenceConfigurator
    {
        IServiceCollection Services { get; }
        IServerManagerTransportConfigurator TransportConfigurator { get; }
    }
}