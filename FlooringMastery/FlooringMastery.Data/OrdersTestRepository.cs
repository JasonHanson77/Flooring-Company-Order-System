using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class OrdersTestRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();
        
        private Order _testOrderOne = new Order
        {
            OrderNumber = 12,
            Date = new DateTime(2016, 10, 31),
            CustomerName = "Brannon",
            State = "PA",
            TaxRate = 6.75m,
            ProductType = "Tile",
            Area = 100.00m,
            CostPerSquareFoot = 3.50m,
            LaborCostPerSquareFoot = 4.15m,
            MaterialCost = 350.00m,
            LaborCost = 415.00m,
            Tax = 51.64m,
            Total = 816.64m
        };

        private Order _testOrderTwo = new Order
        {
            OrderNumber = 14,
            Date = new DateTime(2015, 05, 12),
            CustomerName = "Smith",
            State = "OH",
            TaxRate = 6.25m,
            ProductType = "Wood",
            Area = 100.00m,
            CostPerSquareFoot = 5.15m,
            LaborCostPerSquareFoot = 4.75m,
            MaterialCost = 515.00m,
            LaborCost = 475.00m,
            Tax = 61.88m,
            Total = 1051.88m
        };

        private Order _testOrderThree = new Order
        {
            OrderNumber = 16,
            Date = new DateTime(2016, 10, 31),
            CustomerName = "Price",
            State = "IN",
            TaxRate = 6.00m,
            ProductType = "Carpet",
            Area = 100.00m,
            CostPerSquareFoot = 2.25m,
            LaborCostPerSquareFoot = 2.10m,
            MaterialCost = 225.00m,
            LaborCost = 210.00m,
            Tax = 26.10m,
            Total = 461.10m
        };

        public OrdersTestRepository()
        {
            if (_orders.Count() == 0)
            {
                _orders.Add(_testOrderOne);
                _orders.Add(_testOrderTwo);
                _orders.Add(_testOrderThree);
            }
        }
        
        public List<Order> GetOrders()
        {
            return _orders;
        }

        public void SaveOrder(Order order)
        {
            _orders.Add(order);
        }

        public List<Order> GetOrdersByDate(string date)
        {
            DateTime requestedDate = new DateTime();
            List<Order> dateList = new List<Order>();

            if (DateTime.TryParse(date, out requestedDate))
            {
                foreach (var order in _orders)
                {
                    if (requestedDate == order.Date)
                    {
                        dateList.Add(order);
                    }
                }
            }
            return dateList; 
        }

        public void EditOrder(Order order)
        {
            List<Order> tempList = new List<Order>();
            foreach (var o in _orders)
            {
                if (o.Date != order.Date && o.OrderNumber != order.OrderNumber)
                {
                    tempList.Add(o);
                }
                else if (o.Date == order.Date && o.OrderNumber != order.OrderNumber)
                {
                    tempList.Add(o);
                }
                else if (o.Date != order.Date && o.OrderNumber == order.OrderNumber)
                {
                    tempList.Add(o);
                }
            }

            _orders.Clear();

            _orders.AddRange(tempList);

            SaveOrder(order);
        }

        public void RemoveOrder(DateTime date, int orderNumber)
        {
            Order order = new Order();

            foreach (var o in _orders)
            {
                if (o.Date == date && o.OrderNumber == orderNumber)
                {
                    order = o;
                }

            }

            _orders.Remove(order);
        }

        public void ClearOrders()
        {
            _orders.Clear();
        }
    }
}
