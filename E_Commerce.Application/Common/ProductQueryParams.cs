using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common;

public class ProductQueryParams
{
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public string? SearchText { get; set; }
    public ProductSortingOptions SortingOptions { get; set; }
    public int PageNumber { get; set; }
    private int _pageSize;
    private const int _maxSize = 10;

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = Math.Max(1, Math.Min(_maxSize, value)); }
    }


}
