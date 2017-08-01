using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Models
{
    public interface IOrderRepository
    {
        void SaveOrder(Order order);
        List<Order> GetOrders();
        List<Order> GetOrdersByDate(string date);
        void EditOrder(Order order);
        void RemoveOrder(DateTime date, int orderNumber);
        void ClearOrders();
    }
}
