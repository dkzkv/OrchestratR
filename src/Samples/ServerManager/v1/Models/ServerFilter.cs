using ServerManager.v1.Models.Paging;

namespace ServerManager.v1.Models
{
    public class ServerFilter : PageFilter
    {
        public bool? IsDeleted { get; set; }
    }
}