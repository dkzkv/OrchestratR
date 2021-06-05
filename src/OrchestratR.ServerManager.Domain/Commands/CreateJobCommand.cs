using System;
using MediatR;

namespace OrchestratR.ServerManager.Domain.Commands
{
    public class CreateJobCommand : IRequest<Guid>, ITransactionalCommand
    {
        public CreateJobCommand(string name, string argument)
        {
            if (string.IsNullOrEmpty(name))
                throw new AggregateException($"{nameof(name)} can't be empty or null.");
            
            if (string.IsNullOrEmpty(argument))
                throw new AggregateException($"{nameof(argument)} can't be empty or null.");
            
            Name = name;
            Argument = argument;
        }

        public string Name { get; } 
        public string Argument { get; }
    }
}