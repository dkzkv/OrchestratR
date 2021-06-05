using System;
using System.Collections.Generic;

namespace OrchestratR.Core.Paging
{
    public class Page<T> 
    {
        public int Total { get; }

        public IEnumerable<T> Items { get; }

        public Page(IEnumerable<T> items, int total)
        {
            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total));
            }

            Total = total;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
    }
}