using Microsoft.Extensions.DependencyInjection;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Configurators
{
    internal class ServerManagerPersistenceConfigurator : IServerManagerPersistenceConfigurator
    {
        public ServerManagerPersistenceConfigurator(IServiceCollection services, IServerManagerTransportConfigurator transportConfigurator)
        {
            Services = services;
            TransportConfigurator = transportConfigurator;
        }

        public IServiceCollection Services { get; }
        public IServerManagerTransportConfigurator TransportConfigurator { get; }
    }
}