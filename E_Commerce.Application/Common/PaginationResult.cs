using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class PaginationResult<T>
    {
        public PaginationResult(int pageNumber, int pageSize, int count, T value)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Value = value;
            Count = count;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public T Value { get; set; }
    }
}
