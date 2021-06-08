using System;
using JetBrains.Annotations;
using OrchestratR.Server;

namespace OrchestratR.Extension.RabbitMq
{
    internal class OrchestratrObserverFaultRule : IOrchestratrObserverFaultRule
    {
        public OrchestratrObserverFaultRule([NotNull] string absolutePath, Type errorType)
        {
            AbsolutePath = absolutePath ?? throw new ArgumentNullException(nameof(absolutePath));
            ErrorType = errorType;
        }

        public string AbsolutePath { get; }
        public Type ErrorType { get; }
    }
}