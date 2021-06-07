using JetBrains.Annotations;

namespace ServerManager.Options
{
    [UsedImplicitly]
    internal class ServerManagerOptions
    {
        public string DbConnectionString { get; set; }
        public MqOptions RabbitMq { get; set; }
    }
}