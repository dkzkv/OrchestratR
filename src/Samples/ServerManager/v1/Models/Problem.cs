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
        public string Type { get; set; }
        
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Http status code.
        /// </summary>
        public int StatusCode { get; set; }
        
        /// <summary>
        /// Detailed information of problem.
        /// </summary>
        public string Detail { get; set; }
    }
}