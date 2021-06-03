using System;

namespace OrchestratR.Core.Messages
{
    public abstract class BaseMessage : IMessage
    {
        protected BaseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }
        public Guid CorrelationId { get; }
    }
    
    
    public interface IMessage
    {
        Guid CorrelationId { get; }
    }
    
    public interface IStartJobMessage : IMessage
    {
        Guid Id { get; }
        string JobName { get; }
        string Argument { get; }
        DateTimeOffset CreatedAt { get; }
    }
    
    public interface IStopJobMessage : IMessage
    {
        Guid Id { get; }
    }
    
    public interface ICancelJobMessage : IMessage
    {
        Guid JobId { get; }
    }

    public interface IServerCreatedMessage : IMessage
    {
        Guid Id { get; }
        string Name { get; }
        int MaxWorkersCount { get; }
        DateTimeOffset CreatedAt { get; }
    }
    
    public interface IServerDeletedMessage : IMessage
    {
        Guid Id { get; }
        DateTimeOffset DeletedAt { get; }
    }

    public interface IJobStatusMessage :  IMessage
    {
        Guid Id { get; }
        Guid ServerId { get; }
        OrchestratedJobStatus OrchestratedJobStatus { get; }
        DateTimeOffset UpdatedAt { get; }
    }
}