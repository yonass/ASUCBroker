using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderItem
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class OrderItem:EntityBase<OrderItem> {

        public static List<OrderItem> GetByOrderNumber(string orderNumber) {
            return Table.Where(oi => oi.Order.OrderNumber == orderNumber).ToList();
        }

        public static List<OrderItem> GetByOrderID(int orderID) {
            return Table.Where(oi => oi.OrderID == orderID).ToList();
        }

        public static int GetNextOrdinalNumber(int orderID)
        {
            List<OrderItem> myList = Table.Where(oi => oi.OrderID == orderID).ToList();
            if (myList.Count > 0)
            {
                myList = myList.OrderBy(oi => oi.OrdinalNumber).ToList();
                int lastNumber = myList.Last().OrdinalNumber;
                int nextNumber = lastNumber + 1;
                return nextNumber;
            } else
            {
                return 1;
            }
        }

        public static int GetLastOrdinalNumber(int orderID)
        {
            List<OrderItem> myList = Table.Where(oi => oi.OrderID == orderID).ToList();
            if (myList.Count > 0)
            {
                myList = myList.OrderBy(oi => oi.OrdinalNumber).ToList();
                int lastNumber = myList.Last().OrdinalNumber;
                return lastNumber;
            } else
            {
                return 1;
            }
        }

        public static void UpdateOrdinalNumbersForDeleteOrderItem(int orderID, int deletedNumber)
        {
            List<OrderItem> myList = Table.Where(oi => oi.OrderID == orderID).ToList();
            if (myList.Count > 0)
            {
                myList =  myList.OrderBy(oi=>oi.OrdinalNumber).ToList();
                List<OrderItem> listToBeUpdated = myList.Where(oi => oi.OrdinalNumber > deletedNumber).ToList();
                foreach (OrderItem oi in listToBeUpdated)
                {
                    --oi.OrdinalNumber;
                    OrderItem.Table.Context.SubmitChanges();
                }
            } 
        }


    }
}
