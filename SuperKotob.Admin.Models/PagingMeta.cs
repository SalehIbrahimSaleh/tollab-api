using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core
{
    public class PagingMeta
    {
        public PagingMeta(long page, long totalCount, long pageSize = 20)
        {
            TotalCount = totalCount;
            Page = page;
            PageSize = Convert.ToInt32(pageSize);

            MaxPage = (long)Math.Ceiling(totalCount / (double)pageSize);
            var pageIndex = page > 0 ? page - 1 : page;
            Skip = Convert.ToInt32(pageIndex * pageSize);

        }
        public long TotalCount { get; private set; }
        public long Page { get; private set; }
        public int PageSize { get; private set; }
        public long MaxPage { get; private set; }
        public int Skip { get; private set; }

    }
}
