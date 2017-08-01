using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Tests
{

    [TestFixture]
    public class OrdersTestRepositoryTests
    {
        IOrderRepository repo; 
        [SetUp]
        public void SetUp()
        {
           repo = DIContainer.Kernel.Get<IOrderRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            repo.ClearOrders();
        }

        [Test]
        public void CanLoadOrdersTestData()
        {
            OrderManager manager = new OrderManager(repo);

            string orderDate = "10/31/16";

            LookUpOrderResponse response = manager.LookUpOrder(orderDate);

            Assert.AreEqual(response.orders.Count, 2);

            Assert.IsNotNull(response.orders);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(12, response.orders[0].OrderNumber);
            Assert.AreEqual("Brannon", response.orders[0].CustomerName);
            Assert.AreEqual("PA", response.orders[0].State);
            Assert.AreEqual(6.75m, response.orders[0].TaxRate);
            Assert.AreEqual("Tile", response.orders[0].ProductType);
            Assert.AreEqual(100m, response.orders[0].Area);
            Assert.AreEqual(3.50m, response.orders[0].CostPerSquareFoot);
            Assert.AreEqual(4.15m, response.orders[0].LaborCostPerSquareFoot);
            Assert.AreEqual(350.00m, response.orders[0].MaterialCost);
            Assert.AreEqual(415.00m, response.orders[0].LaborCost);
            Assert.AreEqual(51.64m, response.orders[0].Tax);
            Assert.AreEqual(816.64m, response.orders[0].Total);

            Assert.IsNotNull(response.orders);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(16, response.orders[1].OrderNumber);
            Assert.AreEqual("Price", response.orders[1].CustomerName);
            Assert.AreEqual("IN", response.orders[1].State);
            Assert.AreEqual(6.00m, response.orders[1].TaxRate);
            Assert.AreEqual("Carpet", response.orders[1].ProductType);
            Assert.AreEqual(100m, response.orders[1].Area);
            Assert.AreEqual(2.25m, response.orders[1].CostPerSquareFoot);
            Assert.AreEqual(2.10m, response.orders[1].LaborCostPerSquareFoot);
            Assert.AreEqual(225.00m, response.orders[1].MaterialCost);
            Assert.AreEqual(210.00m, response.orders[1].LaborCost);
            Assert.AreEqual(26.10m, response.orders[1].Tax);
            Assert.AreEqual(461.10m, response.orders[1].Total);
        }
        [Test]
        public void CanAddOrderTest()
        {
            OrderManager manager = new OrderManager(repo);

            OrderRules rules = new OrderRules();

            Order order = new Order();

            order.Date = new DateTime(2018, 2, 2);
            order.CustomerName = "Fred, Johnson";
            order.State = "Pennsylvania";
            order.ProductType = "Carpet";
            order.Area = 100m;

            OrderValidationResponse response = rules.checkOrder(order, true);

            manager.AddOrder(response);

            List<Order> orderList = repo.GetOrdersByDate("2-2-18");

            Assert.IsNotNull(orderList);
            Assert.AreNotEqual(orderList.Count(), 0);
            Assert.AreEqual(orderList[0].OrderNumber, 1);
            Assert.AreEqual(orderList[0].CustomerName, "Fred, Johnson");
            Assert.AreEqual(orderList[0].Date, new DateTime(2018, 2, 2));
            Assert.AreEqual(orderList[0].State, "Pennsylvania");
            Assert.AreEqual(orderList[0].TaxRate, 6.75m);
            Assert.AreEqual(orderList[0].ProductType, "Carpet");
            Assert.AreEqual(orderList[0].CostPerSquareFoot, 2.25m);
            Assert.AreEqual(orderList[0].LaborCostPerSquareFoot, 2.10m);
            Assert.AreEqual(orderList[0].Area, 100m);
            Assert.AreEqual(orderList[0].MaterialCost, 225m);
            Assert.AreEqual(orderList[0].LaborCost, 210);
            Assert.AreEqual(orderList[0].Tax, 29.36m);
            Assert.AreEqual(orderList[0].Total, 464.36m);
        }

        [Test]
        public void CanEditOrderTest()
        {
            OrderManager manager = new OrderManager(repo);

            Order order = new Order();

            order.Date = new DateTime(2016, 10, 31);

            order.OrderNumber = 16;

            List<Order> requestedOrder = requestedOrder = repo.GetOrdersByDate(order.Date.ToShortDateString()).Where(o => o.OrderNumber == order.OrderNumber).ToList();

            Assert.AreEqual(requestedOrder.Count(), 1);
            Assert.AreEqual(16, requestedOrder[0].OrderNumber);
            Assert.AreEqual("Price", requestedOrder[0].CustomerName);
            Assert.AreEqual("IN", requestedOrder[0].State);
            Assert.AreEqual(6.00m, requestedOrder[0].TaxRate);
            Assert.AreEqual("Carpet", requestedOrder[0].ProductType);
            Assert.AreEqual(100m, requestedOrder[0].Area);
            Assert.AreEqual(2.25m, requestedOrder[0].CostPerSquareFoot);
            Assert.AreEqual(2.10m, requestedOrder[0].LaborCostPerSquareFoot);
            Assert.AreEqual(225.00m, requestedOrder[0].MaterialCost);
            Assert.AreEqual(210.00m, requestedOrder[0].LaborCost);
            Assert.AreEqual(26.10m, requestedOrder[0].Tax);
            Assert.AreEqual(461.10, requestedOrder[0].Total);

            requestedOrder[0].State = "OH";
            requestedOrder[0].Area = 200m;
            requestedOrder[0].CustomerName = "Roberts";
            requestedOrder[0].ProductType = "Wood";

            OrderRules rules = new OrderRules();

            OrderValidationResponse response = new OrderValidationResponse();

            response = rules.checkOrder(requestedOrder[0], false);

            manager.EditOrder(response);

            requestedOrder.Clear();

            requestedOrder = repo.GetOrdersByDate(order.Date.ToShortDateString()).Where(o => o.OrderNumber == order.OrderNumber).ToList();

            Assert.AreEqual(requestedOrder.Count(), 1);
            Assert.AreEqual(16, requestedOrder[0].OrderNumber);
            Assert.AreEqual("Roberts", requestedOrder[0].CustomerName);
            Assert.AreEqual("OH", requestedOrder[0].State);
            Assert.AreEqual(6.25m, requestedOrder[0].TaxRate);
            Assert.AreEqual("Wood", requestedOrder[0].ProductType);
            Assert.AreEqual(200m, requestedOrder[0].Area);
            Assert.AreEqual(5.15m, requestedOrder[0].CostPerSquareFoot);
            Assert.AreEqual(4.75m, requestedOrder[0].LaborCostPerSquareFoot);
            Assert.AreEqual(1030.00m, requestedOrder[0].MaterialCost);
            Assert.AreEqual(950.00m, requestedOrder[0].LaborCost);
            Assert.AreEqual(123.75m, requestedOrder[0].Tax);
            Assert.AreEqual(2103.75m, requestedOrder[0].Total);

           
        }

        [Test]
        public void CanRemoveOrderTest()
        {
            OrderManager manager = new OrderManager(repo);

            List<Order> orderList = new List<Order>();

            orderList = repo.GetOrdersByDate("10 31 16");

            Assert.AreEqual(orderList.Count(), 2);

            Assert.IsNotNull(orderList);

            Assert.AreEqual(16, orderList[1].OrderNumber);
            Assert.AreEqual("Price", orderList[1].CustomerName);
            Assert.AreEqual("IN", orderList[1].State);
            Assert.AreEqual(6.00m, orderList[1].TaxRate);
            Assert.AreEqual("Carpet", orderList[1].ProductType);
            Assert.AreEqual(100m, orderList[1].Area);
            Assert.AreEqual(2.25m, orderList[1].CostPerSquareFoot);
            Assert.AreEqual(2.10m, orderList[1].LaborCostPerSquareFoot);
            Assert.AreEqual(225.00m, orderList[1].MaterialCost);
            Assert.AreEqual(210.00m, orderList[1].LaborCost);
            Assert.AreEqual(26.10m, orderList[1].Tax);
            Assert.AreEqual(461.10, orderList[1].Total);

            manager.RemoveOrder(orderList[1].Date, orderList[1].OrderNumber);

            orderList.Clear();

            orderList = repo.GetOrdersByDate("10 31 16");

            Assert.AreEqual(orderList.Count(), 1);

            Assert.IsNotNull(orderList);

            Assert.AreEqual(12, orderList[0].OrderNumber);
            Assert.AreEqual("Brannon", orderList[0].CustomerName);
            Assert.AreEqual("PA", orderList[0].State);
            Assert.AreEqual(6.75m, orderList[0].TaxRate);
            Assert.AreEqual("Tile", orderList[0].ProductType);
            Assert.AreEqual(100m, orderList[0].Area);
            Assert.AreEqual(3.50m, orderList[0].CostPerSquareFoot);
            Assert.AreEqual(4.15m, orderList[0].LaborCostPerSquareFoot);
            Assert.AreEqual(350.00m, orderList[0].MaterialCost);
            Assert.AreEqual(415.00m, orderList[0].LaborCost);
            Assert.AreEqual(51.64m, orderList[0].Tax);
            Assert.AreEqual(816.64m, orderList[0].Total);
        }
    }
}

    

