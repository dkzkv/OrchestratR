using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace OrchestratR.ServerManager.Persistence.Handlers.Specs
{
    internal static class ServerSpecs
    {
        public static Expression<Func<Entities.Server, bool>> ByIsDeleted([CanBeNull] bool? isDeleted)
        {
            if (!isDeleted.HasValue) return x => true;

            return server => server.IsDeleted == isDeleted.Value;
        }
    }
}