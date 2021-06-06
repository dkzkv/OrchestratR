namespace ServerManager.v1.Models
{
    public class Problem
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string Detail { get; set; }
    }
}