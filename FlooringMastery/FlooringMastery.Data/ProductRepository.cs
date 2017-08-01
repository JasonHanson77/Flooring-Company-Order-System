using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _productsList = new List<Product>();
        private string _directoryPath;
        private string _productsFile;

        public ProductRepository(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                _directoryPath = directoryPath;
                _productsFile = _directoryPath + "Products.txt";
            }
            else
            {
                Directory.CreateDirectory(directoryPath);
                string[] files = System.IO.Directory.GetFiles(Settings.SeedDirectoryPath);

                string fileName = "Products.txt";


                foreach (string s in files)
                {

                    if (fileName == System.IO.Path.GetFileName(s))
                    {
                        _productsFile = System.IO.Path.Combine(directoryPath, fileName);
                        System.IO.File.Copy(s, _productsFile, true);
                    }
                }
            }
            _productsFile = _directoryPath + "Products.txt";
            CreateListFromFile(_productsFile);
        }

        public void CreateListFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                sr.ReadLine();
                string line;


                while ((line = sr.ReadLine()) != null)
                {
                    Product product = new Product();

                    string[] columns = line.Split(',');

                    product.ProductType = columns[0];
                    decimal cost = 0m;

                    if (decimal.TryParse(columns[1], out cost))
                    {
                        product.CostPerSquareFoot = cost;
                    }

                    if(decimal.TryParse(columns[2], out cost))
                    {
                        product.LaborCostPerSquareFoot = cost;
                    }

                    _productsList.Add(product);
                }
            }
       }

        public List<Product> GetProducts()
        {
            return _productsList;
        }
    }
}
