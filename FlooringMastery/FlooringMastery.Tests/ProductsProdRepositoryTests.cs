using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
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
    public class ProductsProdRepositoryTests
    {
        [Test]
        public void CanLoadProductsFromFileTest()
        {
            IProductRepository repo = DIContainer.Kernel.Get<IProductRepository>();

            List<Product> products = repo.GetProducts();

            Assert.IsNotNull(products);

            Assert.AreEqual(products[0].ProductType, "Carpet");
            Assert.AreEqual(products[0].CostPerSquareFoot, 2.25m);
            Assert.AreEqual(products[0].LaborCostPerSquareFoot, 2.10m);

            Assert.AreEqual(products[1].ProductType, "Laminate");
            Assert.AreEqual(products[1].CostPerSquareFoot, 1.75m);
            Assert.AreEqual(products[1].LaborCostPerSquareFoot, 2.10m);

            Assert.AreEqual(products[2].ProductType, "Tile");
            Assert.AreEqual(products[2].CostPerSquareFoot, 3.50m);
            Assert.AreEqual(products[2].LaborCostPerSquareFoot, 4.15m);

            Assert.AreEqual(products[3].ProductType, "Wood");
            Assert.AreEqual(products[3].CostPerSquareFoot, 5.15m);
            Assert.AreEqual(products[3].LaborCostPerSquareFoot, 4.75m);

        }

    }
}
