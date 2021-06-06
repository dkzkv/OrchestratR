using System;

namespace OrchestratR.ServerManager.Domain.Common.Exceptions
{
    public class ExistedJobException : Exception
    {
        public ExistedJobException(string jobName) : base($"Job with name: {jobName} already exist.") { }
    }
    
    public class NotExistedJobException : Exception
    {
        public NotExistedJobException(Guid id) : base($"Job with Id: {id} not exist.") { }
    }
}