using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Handlers.Specs
{
    internal static class ServerSpecs
    {
        public static Expression<Func<Entities.Server, bool>> ByIsDeleted([CanBeNull] bool? isDeleted)
        {
            if (!isDeleted.HasValue) return x => true;

            return server => server.IsDeleted == isDeleted.Value;
        }
        
        public static Func<Entities.OrchestratedJob, bool> ByJobStatus(JobLifecycleStatus? status)
        {
            if (!status.HasValue) return x => true;

            return distr => distr.Status == status.Value;
        }
        
        public static Func<Entities.OrchestratedJob, bool> ByExceptJobStatus(JobLifecycleStatus? status)
        {
            if (!status.HasValue) return x => true;

            return distr => distr.Status != status.Value;
        }
    }
}