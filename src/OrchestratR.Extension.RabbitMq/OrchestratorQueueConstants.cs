namespace OrchestratR.Extension.RabbitMq
{
    public static class OrchestratorQueueConstants
    {
        public const string Manager = "orchestratr_manager";
        public const string HeartBeats = "orchestratr_heartbeats";
        public const string OrchestratorJobs = "orchestratr_jobs";
        public const string CancellationJobsPrefix = "orchestratr_job_cancellation_";
    }
}