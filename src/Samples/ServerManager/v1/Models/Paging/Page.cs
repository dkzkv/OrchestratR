using System.Collections.Generic;

namespace ServerManager.v1.Models.Paging
{
    /// <summary>
    /// You know.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        /// Total amount of items according to search criteria.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Seriously?
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}