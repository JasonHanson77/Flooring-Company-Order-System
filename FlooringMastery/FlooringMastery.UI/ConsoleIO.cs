using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI
{
    public static class ConsoleIO
    {
        public static void DisplayOrderDetails(List<Order> orders)
        {
            foreach (var order in orders)
            {
                
                Console.WriteLine("**********************");
                Console.WriteLine($"{order.OrderNumber} |" + $" {order.Date.ToShortDateString()}");
                Console.WriteLine($"Customer Name: {order.CustomerName}");
                Console.WriteLine($"{order.State}");
                Console.WriteLine($"Product: {order.ProductType}");
                Console.WriteLine($"Materials ({order.Area} Sq Ft): {order.MaterialCost:c}");
                Console.WriteLine($"Labor ({order.Area} Sq Ft): {order.LaborCost:c}");
                Console.WriteLine($"Tax: {order.Tax:c}");
                Console.WriteLine($"Total: {order.Total:c}");
                Console.WriteLine("**********************");
                Console.WriteLine();
            }
        }

        public static Order GetOrderNumber(Order order, string header)
        {
            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.Write("Enter an order number of order you would like to edit. Enter Q to quit to quit and return to main menu: ");
                string orderNumberString = Console.ReadLine();

                if (orderNumberString.ToUpper() == "Q")
                {
                   return order = null;
                }
                int orderNumber = 0;
                if (int.TryParse(orderNumberString, out orderNumber))
                {
                    order.OrderNumber = orderNumber;
                    Console.WriteLine("Press any key to continue entering new order data.");
                    Console.ReadKey();
                    Console.Clear();
                    return order;
                }

                Console.WriteLine("Only whole numbers can be entered!");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static Order GetArea(Order order, string header)
        {
            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.Write("Enter an order size. Miniumum order size is 100 sq ft. Enter Q to quit and return to main menu: ");
                string areaString = Console.ReadLine();

                if (areaString.ToUpper() == "Q")
                {
                    return order = null;
                }

                decimal area = 0m;
                if (Decimal.TryParse(areaString, out area))
                {
                    if (area >= 100m)
                    {
                        order.Area = area;
                        Console.WriteLine("Press any key to continue entering new order data.");
                        Console.ReadKey();
                        Console.Clear();
                        return order;
                    }
                }
                Console.WriteLine("Only numbers can be entered and must be over 100 sq ft!");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
                Console.Clear();
            }
        }
            

        public static Order GetProduct(Order order, string header)
        {
            IProductRepository productRepo = DIContainer.Kernel.Get<IProductRepository>();
            List<Product> productList = productRepo.GetProducts();

            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                ConsoleIO.DisplayProducts(productRepo.GetProducts());
                Console.WriteLine();
                Console.Write("Enter an available product from the list for this order. Enter Q to quit to main menu: ");
                string product = Console.ReadLine();

                if (product.ToUpper() == "Q")
                {
                    return order = null;
                }

                foreach (var p in productList)
                {
                    if (p.ProductType == FirstLetterToUpperCase(product))
                    {
                        order.ProductType = FirstLetterToUpperCase(product);
                        Console.WriteLine("Press any key to continue entering new order data.");
                        Console.ReadKey();
                        Console.Clear();
                        return order;
                    }
                }
                Console.WriteLine("You must enter a product from the available products list! Check your spelling and try again!");
                Console.WriteLine("Press any key to try again");
                Console.ReadKey();
            }
        }

        public static Order GetState(Order order, string header)
        {
            ITaxRepository taxRepo = DIContainer.Kernel.Get<ITaxRepository>();
            List<TaxInfo> stateList = taxRepo.GetTaxInfo();

            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                ConsoleIO.DisplayStates(taxRepo.GetTaxInfo());
                Console.WriteLine();
                Console.Write("Enter a state or state abbreviation from the above list for the order. Enter Q to quit to main menu: ");
                string state = Console.ReadLine();

                if (state.ToUpper() == "Q")
                {
                    return order = null;
                }

                if (state.Length > 2)
                {
                    order.State = FirstLetterToUpperCase(state);
                }
                else
                {
                    order.State = state.ToUpper();
                }

                foreach (var s in stateList)
                {
                    if (s.State == order.State)
                    {
                        Console.WriteLine("Press any key to continue entering new order data.");
                        Console.ReadKey();
                        Console.Clear();
                        return order;
                    }
                    else if (s.StateAbbreviation == order.State)
                    {
                        Console.WriteLine("Press any key to continue entering new order data.");
                        Console.ReadKey();
                        Console.Clear();
                        return order;
                    }
                }
                Console.WriteLine("You must enter a state from the list above! Check your spelling and try again.");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }

        public static Order GetName(Order order, string header)
        {
            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.Write("Enter a customer name. Enter Q to quit and return to main menu: ");
                string name = Console.ReadLine();

                if(name.ToUpper() == "Q")
                {
                    return order = null;
                }

                if (!String.IsNullOrWhiteSpace(name))
                {
                    order.CustomerName = FirstLetterToUpperCase(name);
                    Console.WriteLine("Press any key to continue entering new order data.");
                    Console.ReadKey();
                    Console.Clear();
                    return order;
                }
                else
                {
                    Console.WriteLine("Name can not be left blank!");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
            }
        }

        public static Order GetDate(Order order, string header, bool addOrder)
        {
            while (true)
            {
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.WriteLine("Enter an order date for your requested order.");
                Console.WriteLine("Separate month, day and year using (/,-,.) characters or a space. Example (5/12/77 or 5 12 77)");
                Console.Write("You may also enter Q to quit and return to main menu: ");
                string date = Console.ReadLine();

                if (date.ToUpper() == "Q")
                {
                    return order = null;
                }

                DateTime requestedDate = new DateTime();

                if (DateTime.TryParse(date, out requestedDate))
                {
                    order.Date = requestedDate;
                    if (addOrder)
                    {
                        if (order.Date > DateTime.Today)
                        {
                            Console.Clear();
                            return order;
                        }
                        else
                        {
                            Console.WriteLine("Date Entered must be a future date.  Press any key to try again.");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }
                    }

                    Console.Clear();
                    return order;
                }
                else
                {
                    Console.WriteLine("Date not entered in proper format!");
                    Console.WriteLine("Separate month, day and year using (/,-,.) characters or a space. Example (5/12/77 or 5 12 77)!");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public static void DisplayProducts(List<Product> products)
        {
            Console.WriteLine("Currently Available Products");
            Console.WriteLine();
            foreach (var product in products)
            {
                Console.WriteLine("********************");
                Console.WriteLine($"Product: { product.ProductType}");
                Console.WriteLine($"Cost per square foot: {product.CostPerSquareFoot:c}");
                Console.WriteLine($"Labor cost per square foot: {product.LaborCostPerSquareFoot:c}");
                Console.WriteLine("********************");
                Console.WriteLine();
            }
        }

        public static void DisplayStates(List<TaxInfo> states)
        {
            Console.WriteLine("Authorized Sales States");
            Console.WriteLine();
            foreach (var state in states)
            {
                Console.WriteLine("********************");
                Console.WriteLine($"State: { state.State}");
                Console.WriteLine($"State Abbreviation: {state.StateAbbreviation}");
                Console.WriteLine($"TaxRate: {state.TaxRate}");
                Console.WriteLine("********************");
                Console.WriteLine();
            }
        }


        public static string FirstLetterToUpperCase(string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }
    }
}
