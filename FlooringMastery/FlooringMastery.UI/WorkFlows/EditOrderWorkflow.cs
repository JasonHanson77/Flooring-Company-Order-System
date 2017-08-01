using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI.WorkFlows
{
    public class EditOrderWorkflow
    {
        public void Execute()
        {
            IOrderRepository orderRepo = DIContainer.Kernel.Get<IOrderRepository>();
            OrderManager manager = new OrderManager(orderRepo);
            Order order = new Order();
            List<Order> requestedOrder = new List<Order>();
            string header = "Edit an Order";

            while (true)
            {
                Console.Clear();

                if(ConsoleIO.GetOrderNumber(order, header) == null)
                {
                    return;
                }

                if(ConsoleIO.GetDate(order, header, false) == null)
                {
                    return;
                }

                requestedOrder = orderRepo.GetOrdersByDate(order.Date.ToShortDateString()).Where(o => o.OrderNumber == order.OrderNumber).ToList();

                if (requestedOrder.Count() > 0)
                {
                    while (true)
                    {
                        ConsoleIO.DisplayOrderDetails(requestedOrder);

                        Console.WriteLine("Only customer's name, state, product and area can be edited.");
                        Console.WriteLine("Pressing enter will leave each field in it's current state.");
                        Console.WriteLine("Press any key to continue editing order displayed above.");
                        Console.ReadKey();
                        Console.Clear();

                        if(EditName(requestedOrder, header).Count() == 0)
                        {
                            return;
                        }

                        if(EditState(requestedOrder, header).Count() == 0)
                        {
                            return;
                        }

                        if(EditProduct(requestedOrder, header).Count() == 0)
                        {
                            return;
                        }

                        if(EditArea(requestedOrder, header).Count() == 0)
                        {
                            return;
                        }

                        foreach (var o in requestedOrder)
                        {
                            OrderRules rulesResponse = new OrderRules();
                            OrderValidationResponse response = rulesResponse.checkOrder(o, false);

                            if (response.Success)
                            {
                                List<Order> checkedOrder = new List<Order>();
                                checkedOrder.Add(response.order);
                                Console.Clear();
                                Console.WriteLine("Confirm Edited Order Details");
                                Console.WriteLine("****************************");
                                ConsoleIO.DisplayOrderDetails(checkedOrder);
                                Console.WriteLine("****************************");
                                Console.WriteLine();
                                while (true)
                                {
                                    Console.Write("Would you like to save the edited order? Y or N: ");
                                    string saveOrder = Console.ReadLine().ToUpper();

                                    switch (saveOrder)
                                    {
                                        case "Y":
                                            manager.EditOrder(response);
                                            Console.WriteLine("Edited Order has been saved!");
                                            Console.WriteLine("Press any key to continue.");
                                            Console.ReadKey();
                                            return;
                                        case "N":
                                            Console.WriteLine("Press any key to return to main Menu.");
                                            Console.ReadKey();
                                            return;
                                        default:
                                            Console.WriteLine("Enter Y to confirm adding order or N to return to main menu.");
                                            Console.WriteLine("Press any key to continue");
                                            Console.ReadKey();
                                            continue;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Requested order not found!");
                    Console.Write("Press any key to enter another date/order number combination or enter Q to return to main menu: ");
                    string continueEdit = Console.ReadLine();

                    if (continueEdit.ToUpper() == "Q")
                    {
                        return;
                    }
                }
            }
        }
        public static List<Order> EditName(List<Order> orders, string header)
        {
            while (true)
            {

                ConsoleIO.DisplayOrderDetails(orders);
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.WriteLine("You may press enter to leave current data in field.");
                Console.Write("Or enter a new customer name. Enter Q to quit and return to main menu: ");
                string name = Console.ReadLine();

                if (name.ToUpper() == "Q")
                {
                    orders.Clear();
                    return orders;
                }

                if (String.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Current name in order will be left as is.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    return orders;
                }

                foreach (var order in orders)
                {
                    order.CustomerName = ConsoleIO.FirstLetterToUpperCase(name);
                }
                Console.Clear();
                ConsoleIO.DisplayOrderDetails(orders);
                Console.WriteLine("Press any key to continue entering new order data.");
                Console.ReadKey();
                Console.Clear();
                return orders;
            }
        }


        public static List<Order> EditState(List<Order> orders, string header)
        {
            ITaxRepository taxRepo = DIContainer.Kernel.Get<ITaxRepository>();
            List<TaxInfo> stateList = taxRepo.GetTaxInfo();

            while (true)
            {
                ConsoleIO.DisplayOrderDetails(orders);
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                ConsoleIO.DisplayStates(stateList);
                Console.WriteLine();
                Console.WriteLine("You may press enter to leave current data in field.");
                Console.Write(" Or enter a new state or state abbreviation from the above list for the order. Enter Q to quit to main menu: ");
                string state = Console.ReadLine();

                if (state.ToUpper() == "Q")
                {
                    orders.Clear();
                    return orders;
                }

                if (String.IsNullOrWhiteSpace(state))
                {
                    Console.WriteLine("Current state in order will be left as is.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    return orders;
                }

                foreach (var order in orders)
                {
                    if (state.Length > 2)
                    {
                        order.State = ConsoleIO.FirstLetterToUpperCase(state);
                    }
                    else
                    {
                        order.State = state.ToUpper();
                    }

                    foreach (var s in stateList)
                    {
                        if (s.State == order.State)
                        {
                            OrderValidationResponse response = new OrderValidationResponse();
                            OrderRules rules = new OrderRules();
                            response = rules.checkOrder(order, false);
                            List<Order> updatedList = new List<Order>();

                            if (response.Success)
                            {
                                updatedList.Add(response.order);
                                Console.Clear();
                                ConsoleIO.DisplayOrderDetails(updatedList);
                                Console.WriteLine("Press any key to continue entering new order data.");
                                Console.ReadKey();
                                Console.Clear();
                                return updatedList;
                            }
                        }
                        else if (s.StateAbbreviation == order.State)
                        {
                            OrderValidationResponse response = new OrderValidationResponse();
                            OrderRules rules = new OrderRules();
                            response = rules.checkOrder(order, false);
                            List<Order> updatedList = new List<Order>();

                            if (response.Success)
                            {
                                updatedList.Add(response.order);
                                Console.Clear();
                                ConsoleIO.DisplayOrderDetails(updatedList);
                                Console.WriteLine("Press any key to continue entering new order data.");
                                Console.ReadKey();
                                Console.Clear();
                                return updatedList;
                            }
                        }
                    }
                    Console.WriteLine("You must enter a state from the list above! Check your spelling and try again.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }
        public static List<Order> EditProduct(List<Order> orders, string header)
        {
            IProductRepository productRepo = DIContainer.Kernel.Get<IProductRepository>();
            List<Product> productList = productRepo.GetProducts();

            while (true)
            {
                ConsoleIO.DisplayOrderDetails(orders);
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                ConsoleIO.DisplayProducts(productList);
                Console.WriteLine();
                Console.WriteLine("You may press enter to leave current data in field.");
                Console.Write(" Or enter a new product from the above list for the order. Enter Q to quit to main menu: ");
                string product = Console.ReadLine();

                if (product.ToUpper() == "Q")
                {
                    orders.Clear();
                    return orders;
                }

                if (String.IsNullOrWhiteSpace(product))
                {
                    Console.WriteLine("Current product in order will be left as is.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    return orders;
                }

                foreach (var order in orders)
                {
                    order.ProductType = ConsoleIO.FirstLetterToUpperCase(product);


                    foreach (var p in productList)
                    {
                        if (p.ProductType == order.ProductType)
                        {
                            OrderValidationResponse response = new OrderValidationResponse();
                            OrderRules rules = new OrderRules();
                            response = rules.checkOrder(order, false);
                            List<Order> updatedList = new List<Order>();

                            if (response.Success)
                            {
                                updatedList.Add(response.order);
                                Console.Clear();
                                ConsoleIO.DisplayOrderDetails(updatedList);
                                Console.WriteLine("Press any key to continue entering new order data.");
                                Console.ReadKey();
                                Console.Clear();
                                return updatedList;
                            }
                        }
                    }
                    Console.WriteLine("You must enter a product from the list above! Check your spelling and try again.");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }
        public static List<Order> EditArea(List<Order> orders, string header)
        {
            while (true)
            {
                ConsoleIO.DisplayOrderDetails(orders);
                Console.WriteLine(header);
                Console.WriteLine("--------------------------");
                Console.WriteLine();
                Console.WriteLine("You may press enter to leave current data in field.");
                Console.Write(" Or enter a new area amount for your order. Enter Q to quit to main menu: ");
                string areaString = Console.ReadLine();

                if (areaString.ToUpper() == "Q")
                {
                    orders.Clear();
                    return orders;
                }

                if (String.IsNullOrWhiteSpace(areaString))
                {
                    Console.WriteLine("Current area amount in order will be left as is.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                    return orders;
                }

                decimal area = 0m;
                if (decimal.TryParse(areaString, out area))
                {
                    foreach (var order in orders)
                    {
                        order.Area = area;

                        if (order.Area >= 100m)
                        {
                            OrderValidationResponse response = new OrderValidationResponse();
                            OrderRules rules = new OrderRules();
                            response = rules.checkOrder(order, false);
                            List<Order> updatedList = new List<Order>();

                            if (response.Success)
                            {
                                updatedList.Add(response.order);
                                Console.Clear();
                                ConsoleIO.DisplayOrderDetails(updatedList);
                                Console.WriteLine("Press any key to continue entering new order data.");
                                Console.ReadKey();
                                Console.Clear();
                                return updatedList;
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("You must enter a number in decimal format 100 or above!");
                    Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();
                }
            }
            ;
        }
    }
}
   
