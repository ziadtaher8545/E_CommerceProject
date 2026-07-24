using E_Commerce.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(Expression<Func<Order, bool>> criteria) : base(criteria)
        {

            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);

        }
        public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);



        }
    }
}
