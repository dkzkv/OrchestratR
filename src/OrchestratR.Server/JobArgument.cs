namespace OrchestratR.Server
{
    public class JobArgument
    {
        public JobArgument(string name, string argument)
        {
            Name = name;
            Argument = argument;
        }
        
        public string Name { get; }
        public string Argument { get; }
    }
}