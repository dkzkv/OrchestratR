namespace ServerManager.v1.Models
{
    /// <summary>
    /// Forbidden action, error, or conflict or etc...
    /// </summary>
    public class Problem
    {
        /// <summary>
        /// Type of error.
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }
        
        
        /// <summary>
        /// Detailed information of problem.
        /// </summary>
        public string Details { get; set; }
    }
}