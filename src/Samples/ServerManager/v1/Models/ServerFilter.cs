using ServerManager.v1.Models.Paging;

namespace ServerManager.v1.Models
{
    /// <summary>
    /// Server filter.
    /// </summary>
    public class ServerFilter : PageFilter
    {
        /// <summary>
        /// Is server deleted.
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}