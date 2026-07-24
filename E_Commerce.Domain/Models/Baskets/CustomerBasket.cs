using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models.Baskets
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; 
        public ICollection<BasketItem> Items { get; set; } = [];
        public string? ClientSecert { get; set; }
        public string? PaymentIntendId { get; set; }
        public int DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
