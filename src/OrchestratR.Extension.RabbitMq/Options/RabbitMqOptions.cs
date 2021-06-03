using System;
using JetBrains.Annotations;

namespace OrchestratR.Extension.RabbitMq.Options
{
    [UsedImplicitly]
    public class RabbitMqOptions
    {
        public RabbitMqOptions(string host, string userName, string password)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentException("Host not valid for rabbit mq.");
            Host = host;
            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Invalid username or password.");
            UserName = userName;
            Password = password;
        }

        public string Host { get; }
        public string UserName { get; }
        public string Password { get; }
    }
}