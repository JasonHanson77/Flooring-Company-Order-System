using FlooringMastery.BLL;
using FlooringMastery.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI.WorkFlows
{
    public class RemoveOrderWorkflow
    {
        public void Execute()
        {
            IOrderRepository orderRepo = DIContainer.Kernel.Get<IOrderRepository>();
            OrderManager manager = new OrderManager(orderRepo);
            Order order = new Order();
            List<Order> requestedOrder = new List<Order>();
            string header = "Remove an Order";

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

                requestedOrder = requestedOrder = orderRepo.GetOrdersByDate(order.Date.ToShortDateString()).Where(o => o.OrderNumber == order.OrderNumber).ToList();

                if (requestedOrder.Count() > 0)
                {
                    while (true)
                    {
                        ConsoleIO.DisplayOrderDetails(requestedOrder);

                        Console.Write("Is this the order you would like to remove? Y or N: ");
                        string removeOrder = Console.ReadLine().ToUpper();

                        switch (removeOrder)
                        {
                            case "Y":
                                manager.RemoveOrder(requestedOrder[0].Date, requestedOrder[0].OrderNumber);
                                Console.WriteLine("Order has been deleted!");
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
                else
                {
                    while (true)
                    {
                        Console.Write("Order requested not found! Try again? Y or N: ");
                        string answer = Console.ReadLine();

                        switch (answer.ToUpper())
                        {
                            case "Y":
                                break;
                            case "N":
                                return;
                            default:
                                Console.WriteLine("Answer Y to look up another order. N to return to main menu");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                        }
                        break;
                    }
                }
            }
        }
    }
}
