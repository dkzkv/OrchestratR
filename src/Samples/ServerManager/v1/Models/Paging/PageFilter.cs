namespace ServerManager.v1.Models.Paging
{
    /// <summary>
    /// Common page filters.
    /// </summary>
    public class PageFilter
    {
        /// <summary>
        /// Index of the first element. 
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Count of items on the page. 
        /// </summary>
        public int? Count { get; set; }
    }
}