using System;

namespace OrchestratR.Server
{
    public interface IOrchestratrObserverFaultRule
    {
        string AbsolutePath { get; }
        public Type ErrorType { get; }
    }
}