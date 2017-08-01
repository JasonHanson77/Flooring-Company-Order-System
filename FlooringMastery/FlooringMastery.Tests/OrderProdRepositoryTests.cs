using NUnit.Framework;
using FlooringMastery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FlooringMastery.Models;
using FlooringMastery.BLL;
using Ninject;
using FlooringMastery.Models.Responses;

namespace FlooringMastery.Tests
{
    [TestFixture]
    public class OrderProdRepositoryTests
    {
        IOrderRepository repo;

        [SetUp]
        public void Setup()
        {
            
            if (Directory.Exists(Settings.DirectoryPath))
            {
                Directory.Delete(Settings.DirectoryPath, true);
            }

            Directory.CreateDirectory(Settings.DirectoryPath);

            string[] files = System.IO.Directory.GetFiles(Settings.SeedDirectoryPath);

            string fileName;
            string destFile;
            
            foreach (string s in files)
            {
                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(Settings.DirectoryPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
            repo = DIContainer.Kernel.Get<IOrderRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            repo.ClearOrders();
        }

        [Test]
        public void CanLoadOrderTestData()
        {
            OrderManager manager = new OrderManager(repo);

            string orderDate = "05 01 17";

            LookUpOrderResponse response = manager.LookUpOrder(orderDate);

            Assert.AreEqual(response.orders.Count, 4);

            Assert.IsNotNull(response.orders);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.orders[1].OrderNumber);
            Assert.AreEqual("Smith", response.orders[1].CustomerName);
            Assert.AreEqual("PA", response.orders[1].State);
            Assert.AreEqual(6.75m, response.orders[1].TaxRate);
            Assert.AreEqual("Tile", response.orders[1].ProductType);
            Assert.AreEqual(100m, response.orders[1].Area);
            Assert.AreEqual(3.50m, response.orders[1].CostPerSquareFoot);
            Assert.AreEqual(4.15m, response.orders[1].LaborCostPerSquareFoot);
            Assert.AreEqual(350.00m, response.orders[1].MaterialCost);
            Assert.AreEqual(415.00m, response.orders[1].LaborCost);
            Assert.AreEqual(51.64m, response.orders[1].Tax);
            Assert.AreEqual(816.64m, response.orders[1].Total);
        }

        [Test]
        public void CanAddOrderTest()
        {
            OrderManager manager = new OrderManager(repo);

            Order order = new Order();

            order.Date = new DateTime(2019, 1, 1);
            order.CustomerName = "Smith N' Co., Inc.";
            order.ProductType = "Wood";
            order.Area = 101m;
            order.State = "Ohio";

            OrderRules rules = new OrderRules();

            OrderValidationResponse response = rules.checkOrder(order, true);

            manager.AddOrder(response);

            List<Order> checkOrderList = repo.GetOrdersByDate("1/1/19");

            Assert.IsNotNull(checkOrderList);
            Assert.IsTrue(checkOrderList.Count() != 0);
            Assert.AreEqual(checkOrderList.Count(), 1);
            Assert.AreEqual(checkOrderList[0].OrderNumber, 1);
            Assert.AreEqual(checkOrderList[0].CustomerName, "Smith N' Co., Inc.");
            Assert.AreEqual(checkOrderList[0].Date, new DateTime(2019, 1, 1));
            Assert.AreEqual(checkOrderList[0].State, "Ohio");
            Assert.AreEqual(checkOrderList[0].TaxRate, 6.25m);
            Assert.AreEqual(checkOrderList[0].ProductType, "Wood");
            Assert.AreEqual(checkOrderList[0].CostPerSquareFoot, 5.15m);
            Assert.AreEqual(checkOrderList[0].LaborCostPerSquareFoot, 4.75m);
            Assert.AreEqual(checkOrderList[0].MaterialCost, 520.15m);
            Assert.AreEqual(checkOrderList[0].LaborCost, 479.75);
            Assert.AreEqual(checkOrderList[0].Area, 101m);
            Assert.AreEqual(checkOrderList[0].Tax, 62.49m);
            Assert.AreEqual(checkOrderList[0].Total, 1062.39m);
        }

        [Test]
        public void CanEditOrderTest()
        {
            OrderManager manager = new OrderManager(repo);

            Order order = new Order();

            order.Date = new DateTime(2017, 5, 1);

            order.OrderNumber = 2;

            List<Order> requestedOrder = requestedOrder = repo.GetOrdersByDate(order.Date.ToShortDateString()).Where(o => o.OrderNumber == order.OrderNumber).ToList();

            Assert.AreEqual(requestedOrder.Count(), 1);
            Assert.AreEqual(2, requestedOrder[0].OrderNumber);
            Assert.AreEqual("Smith", requestedOrder[0].CustomerName);
            Assert.AreEqual("PA", requestedOrder[0].State);
            Assert.AreEqual(6.75m, requestedOrder[0].TaxRate);
            Assert.AreEqual("Tile", requestedOrder[0].ProductType);
            Assert.AreEqual(100m, requestedOrder[0].Area);
            Assert.AreEqual(3.50m, requestedOrder[0].CostPerSquareFoot);
            Assert.AreEqual(4.15m, requestedOrder[0].LaborCostPerSquareFoot);
            Assert.AreEqual(350.00m, requestedOrder[0].MaterialCost);
            Assert.AreEqual(415.00m, requestedOrder[0].LaborCost);
            Assert.AreEqual(51.64m, requestedOrder[0].Tax);
            Assert.AreEqual(816.64m, requestedOrder[0].Total);

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
            Assert.AreEqual(2, requestedOrder[0].OrderNumber);
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

            orderList = repo.GetOrdersByDate("5 1 17");

            Assert.AreEqual(orderList.Count(), 4);

            Assert.IsNotNull(orderList);

            Assert.AreEqual(2, orderList[1].OrderNumber);
            Assert.AreEqual("Smith", orderList[1].CustomerName);
            Assert.AreEqual("PA", orderList[1].State);
            Assert.AreEqual(6.75m, orderList[1].TaxRate);
            Assert.AreEqual("Tile", orderList[1].ProductType);
            Assert.AreEqual(100m, orderList[1].Area);
            Assert.AreEqual(3.50m, orderList[1].CostPerSquareFoot);
            Assert.AreEqual(4.15m, orderList[1].LaborCostPerSquareFoot);
            Assert.AreEqual(350.00m, orderList[1].MaterialCost);
            Assert.AreEqual(415.00m, orderList[1].LaborCost);
            Assert.AreEqual(51.64m, orderList[1].Tax);
            Assert.AreEqual(816.64m, orderList[1].Total);

            manager.RemoveOrder(orderList[1].Date, orderList[1].OrderNumber);

            orderList.Clear();

            orderList = repo.GetOrdersByDate("5 1 17");

            Assert.AreEqual(orderList.Count(), 3);

            Assert.IsNotNull(orderList);

            Assert.AreEqual(3, orderList[1].OrderNumber);
            Assert.AreEqual("Jones", orderList[1].CustomerName);
            Assert.AreEqual("MI", orderList[1].State);
            Assert.AreEqual(5.75m, orderList[1].TaxRate);
            Assert.AreEqual("Laminate", orderList[1].ProductType);
            Assert.AreEqual(100m, orderList[1].Area);
            Assert.AreEqual(1.75m, orderList[1].CostPerSquareFoot);
            Assert.AreEqual(2.10m, orderList[1].LaborCostPerSquareFoot);
            Assert.AreEqual(175.00m, orderList[1].MaterialCost);
            Assert.AreEqual(210.00m, orderList[1].LaborCost);
            Assert.AreEqual(22.14m, orderList[1].Tax);
            Assert.AreEqual(407.14m, orderList[1].Total);
        }
    }
}
