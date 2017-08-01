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
    public class AddOrderWorkflow
    {
        public void Execute()
        {
            IOrderRepository orderRepo = DIContainer.Kernel.Get<IOrderRepository>();
            OrderManager manager = new OrderManager(orderRepo);
            Order order = new Order();
            string header = "Add An Order -- Date entered for new orders must be a future date";

            Console.Clear();
            while (true)
            {
                if(ConsoleIO.GetDate(order, header, true) == null)
                {
                    return;
                }
                
               
                if(ConsoleIO.GetName(order, header) == null)
                {
                    return;
                }

                if(ConsoleIO.GetState(order, header) == null)
                {
                    return;
                }

                if(ConsoleIO.GetProduct(order, header) == null)
                {
                    return;
                }

                if(ConsoleIO.GetArea(order, header) == null)
                {
                    return;
                }
                else
                {
                    break;
                }
            }
            
            OrderRules rulesResponse = new OrderRules();
            OrderValidationResponse response = rulesResponse.checkOrder(order, true);

            if (response.Success)
            {
                List<Order> checkedOrder = new List<Order>();
                checkedOrder.Add(response.order);
                Console.Clear();
                Console.WriteLine("Confirm Order Details");
                Console.WriteLine("****************************");
                ConsoleIO.DisplayOrderDetails(checkedOrder);
                Console.WriteLine("****************************");
                Console.WriteLine();
                while (true)
                {
                    Console.Write("Would you like to place the above order? Y or N: ");
                    string saveOrder = Console.ReadLine().ToUpper();

                    switch (saveOrder)
                    {
                        case "Y":
                            manager.AddOrder(response);
                            Console.WriteLine("Order has been added!");
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
                Console.WriteLine(response.Message);
                Console.WriteLine("Press any key to return to main menu and start over.");
                Console.ReadKey();
                return;
            }
        }
    }
}


    
 
    

