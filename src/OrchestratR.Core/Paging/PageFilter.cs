namespace OrchestratR.Core.Paging
{
    public class PageFilter : IPageFilter
    {
        /// <summary>
        ///     page count
        /// </summary>
        public int Count { get; set; } = 20;

        /// <summary>
        ///     page offset
        /// </summary>
        public int Offset { get; set; } = 0;
    }
}