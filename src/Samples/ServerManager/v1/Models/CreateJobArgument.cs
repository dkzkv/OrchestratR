namespace ServerManager.v1.Models
{
    /// <summary>
    /// That's all what you need to create new job.
    /// </summary>
    public class CreateJobArgument
    {
        /// <summary>
        /// Unique job name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Argument for your job.
        /// </summary>
        public string Argument { get; set; }
    }
}