using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using OrchestratR.ServerManager.Domain.Models;

namespace OrchestratR.ServerManager.Persistence.Handlers.Specs
{
    internal static class JobSpecs
    {
        public static Expression<Func<Entities.OrchestratedJob, bool>> ByName([CanBeNull] string name)
        {
            if (string.IsNullOrEmpty(name)) return x => true;

            return distr => distr.Name.ToLower().StartsWith(name.ToLower());
        }
        
        public static Expression<Func<Entities.OrchestratedJob, bool>> ByStatus(JobLifecycleStatus? status)
        {
            if (!status.HasValue) return x => true;

            return distr => distr.Status == status.Value;
        }
        
        public static Expression<Func<Entities.OrchestratedJob, bool>> ByExceptStatus(JobLifecycleStatus? exceptStatus)
        {
            if (!exceptStatus.HasValue) return x => true;

            return distr => distr.Status != exceptStatus.Value;
        }
    }
}