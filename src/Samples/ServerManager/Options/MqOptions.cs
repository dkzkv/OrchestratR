using JetBrains.Annotations;

namespace ServerManager.Options
{
    [UsedImplicitly]
    internal class MqOptions
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}