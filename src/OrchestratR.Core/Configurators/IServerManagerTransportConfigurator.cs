using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace OrchestratR.Core.Configurators
{
    public interface IServerManagerTransportConfigurator
    {
        IServiceCollectionBusConfigurator BusConfigurator { get; }
    }
}