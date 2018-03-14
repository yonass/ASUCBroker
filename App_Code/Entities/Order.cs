using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Broker.Controllers;

/// <summary>
/// Summary description for Order
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Order:EntityBase<Order> {
        public static Order GetByOrderNumber(string orderNumber) {
            return Table.Where(o => o.OrderNumber == orderNumber).SingleOrDefault();
        }

        public static Order GetOrder(int id)
        {
            if (id > 0)
            {
                return Order.Get(id);
            } else
            {
                Order o = new Order();
                o.OrderNumber = CodeGenerator.OrderCodeGenerator();
                return o;
            }
        }

        public static void ValidateOrderNumber(string number) {
            Order o = new Order();
            o.OrderNumber = number;
            o.ValidateNumber();
        }

        public void ValidateNumber() {
            Order o = GetByOrderNumber(this.OrderNumber);
            if (o != null) {
                ValidationErrors.Add("OrderNumberValidator", "Постои налог со овој број");
            }
        }

        
    
    }
}
