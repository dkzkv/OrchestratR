using MassTransit.ExtensionsDependencyInjectionIntegration;
using OrchestratR.Core.Configurators;

namespace OrchestratR.ServerManager.Configurators
{
    internal class ServerManagerTransportConfigurator : IServerManagerTransportConfigurator
    {
        public ServerManagerTransportConfigurator(IServiceCollectionBusConfigurator busConfigurator)
        {
            BusConfigurator = busConfigurator;
        }

        public IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}