namespace OrchestratR.Core.Paging
{
    /// <summary>
    /// Page filter.
    /// </summary>
    public interface IPageFilter
    {
        /// <summary>
        /// How many items will be skipped.
        /// </summary>
        int Offset { get; }

        /// <summary>
        /// How many items will be taken.
        /// </summary>
        int Count { get; }
    }
}