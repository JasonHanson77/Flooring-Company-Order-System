using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.Models;

namespace FlooringMastery.Data
{
    public class ProductsTestRepository : IProductRepository
    {
        private static List<Product> _products = new List<Product>();

        private Product Carpet = new Product
        {
            ProductType = "Carpet",
            CostPerSquareFoot = 2.25m,
            LaborCostPerSquareFoot = 2.10m
        };

        private Product Laminate = new Product
        {
            ProductType = "Laminate",
            CostPerSquareFoot = 1.75m,
            LaborCostPerSquareFoot = 2.10m
        };

        private Product Tile = new Product
        {
            ProductType = "Tile",
            CostPerSquareFoot = 3.50m,
            LaborCostPerSquareFoot = 4.15m
        };

        private Product Wood = new Product
        {
            ProductType = "Wood",
            CostPerSquareFoot = 5.15m,
            LaborCostPerSquareFoot = 4.75m
        };

        public ProductsTestRepository()
        {
            if (_products.Count() == 0)
            {
                _products.Add(Wood);
                _products.Add(Carpet);
                _products.Add(Tile);
                _products.Add(Laminate);
            }
        }
        public List<Product> GetProducts()
        {
            return _products;
        }
    }
}
