using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.Common.Models
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; private set; }
        public int TotalPageCount { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }

        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (decimal)pageSize);
        }
    }
}
