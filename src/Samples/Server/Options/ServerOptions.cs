namespace Server.Options
{
    internal class ServerOptions
    {
        public string ServerName { get; set; }
        public int MaxWorkersCount { get; set; }
        public MqOptions RabbitMq { get; set; }
    }
}