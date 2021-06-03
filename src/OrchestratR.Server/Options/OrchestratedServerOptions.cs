using JetBrains.Annotations;

namespace OrchestratR.Server.Options
{
    [UsedImplicitly]
    public class OrchestratedServerOptions
    {
        public OrchestratedServerOptions(string orchestratorName, int maxWorkersCount)
        {
            OrchestratorName = orchestratorName;
            MaxWorkersCount = maxWorkersCount;
        }
        
        public string OrchestratorName { get; }
        public int MaxWorkersCount { get; }
    }
}