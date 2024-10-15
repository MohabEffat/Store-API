using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specs.OrderSpecs
{
    public class OrderWithItemSpecifications : Specifications<Order>
    {
        public OrderWithItemSpecifications(string buyerEmail)
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.Items);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderWithItemSpecifications(Guid id)
    : base(order => order.Id == id)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.Items);
        }
    }
}
