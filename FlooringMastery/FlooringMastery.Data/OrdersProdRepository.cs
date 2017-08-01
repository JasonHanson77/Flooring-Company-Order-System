using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class OrdersProdRepository : IOrderRepository
    {
        private string _directoryPath;
        private List<Order> _orders = new List<Order>();

        public OrdersProdRepository(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                _directoryPath = directoryPath;

                string[] files = System.IO.Directory.GetFiles(Settings.SeedDirectoryPath);

                string fileName;
                string destFile;

                foreach (string s in files)
                {

                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(directoryPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
            else
            {
                Directory.CreateDirectory(directoryPath);
                string[] files = System.IO.Directory.GetFiles(Settings.SeedDirectoryPath);

                string fileName;
                string destFile;

                foreach (string s in files)
                {

                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(directoryPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
                _directoryPath = directoryPath;
            }
            CreateListFromFile(_directoryPath);
        }

        public void CreateListFromFile(string directoryPath)
        {
            string[] fileNames = Directory.GetFiles(_directoryPath, "Orders_*.txt");

            foreach (var name in fileNames)
            {
                DateTime fileDate = new DateTime();
                string dateString = name.Substring(name.LastIndexOf('_') + 1, 8);
                string orderDate = dateString.Substring(0, 2) + "/" + dateString.Substring(2, 2) + "/" + dateString.Substring(4);

                if (DateTime.TryParse(orderDate, out fileDate))
                {
                    using (StreamReader sr = new StreamReader(name))
                    {
                        sr.ReadLine();
                        string line;


                        while ((line = sr.ReadLine()) != null)
                        {

                            Order order = new Order();

                            order.Date = fileDate;

                            List<string> columns = line.Split(',').ToList();

                            int OrderNumber;

                            if (int.TryParse(columns[0], out OrderNumber))
                            {
                                order.OrderNumber = OrderNumber;
                            }
                            if (columns[1].First() == '"')
                            {
                                order.CustomerName = RebuildNameWithCommaFromCSV(columns[1], columns[2]);
                                columns.Remove(columns[2]);
                            }
                            else
                            {
                                order.CustomerName = columns[1];
                            }

                            order.State = columns[2];

                            decimal TaxRate;

                            if (decimal.TryParse(columns[3], out TaxRate))
                            {
                                order.TaxRate = TaxRate;
                            }


                            order.ProductType = columns[4];

                            decimal Area;

                            if (decimal.TryParse(columns[5], out Area))
                            {
                                order.Area = Area;
                            }

                            decimal CostPerSquareFoot;

                            if (decimal.TryParse(columns[6], out CostPerSquareFoot))
                            {
                                order.CostPerSquareFoot = CostPerSquareFoot;
                            }

                            decimal LaborCostPerSquareFoot;

                            if (decimal.TryParse(columns[7], out LaborCostPerSquareFoot))
                            {
                                order.LaborCostPerSquareFoot = LaborCostPerSquareFoot;
                            }

                            decimal MaterialCost;

                            if (decimal.TryParse(columns[8], out MaterialCost))
                            {
                                order.MaterialCost = MaterialCost;
                            }

                            decimal LaborCost;

                            if (decimal.TryParse(columns[9], out LaborCost))
                            {
                                order.LaborCost = LaborCost;
                            }

                            decimal Tax;

                            if (decimal.TryParse(columns[10], out Tax))
                            {
                                order.Tax = Tax;
                            }

                            decimal Total;

                            if (decimal.TryParse(columns[11], out Total))
                            {
                                order.Total = Total;
                            }

                            _orders.Add(order);

                        }
                    }
                }
            }
        }





        public void SaveOrder(Order order)
        {
            _orders.Add(order);
            CreateOrdersFile(_orders);
            ClearOrders();
            CreateListFromFile(_directoryPath);
        }

        public List<Order> GetOrders()
        {
            return _orders;
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

        private string CreateCsvForOrders(Order order)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", order.OrderNumber,
                InsertCommasAroundNameForCSV(order.CustomerName), order.State, order.TaxRate, order.ProductType,
                order.Area, order.CostPerSquareFoot, order.LaborCostPerSquareFoot,
                order.MaterialCost, order.LaborCost, order.Tax, order.Total);
        }

        private void CreateOrdersFile(List<Order> orders)
        {
            string dateString = orders.LastOrDefault().Date.ToString("MMddyyyy");
            string filePath = _directoryPath + "Orders_" + dateString + ".txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }


            using (StreamWriter sr = new StreamWriter(filePath))
            {
                sr.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");
                foreach (var order in orders)
                {
                    if (order.Date == orders.LastOrDefault().Date)
                        sr.WriteLine(CreateCsvForOrders(order));
                }
            }
        }

        private string InsertCommasAroundNameForCSV(string name)
        {
            if (name.Contains(","))
            {
                name = '"' + name + '"';
            }
            return name;
        }
        private string RebuildNameWithCommaFromCSV(string first, string second)
        {
            string newString = "";
            if (first.First() == '"')
            {
                newString = first.Remove(0, 1) + "," + second.Remove(second.Length - 1);
            }
            else
            {
                newString = first;
            }
            return newString;
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
            List<Order> tempList = new List<Order>();
            Order order = new Order();

            foreach (var o in _orders)
            {
                if (o.Date == date && o.OrderNumber != orderNumber)
                {
                    tempList.Add(o);
                }

                if (o.Date == date && o.OrderNumber == orderNumber)
                {
                    order = o;
                }

            }


            _orders.Remove(order);

            CreateOrdersFile(tempList);
        }

        public void ClearOrders()
        {
            _orders.Clear();
        }
    }
}

