using FlooringMastery.BLL;
using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI.WorkFlows
{
    public class LookUpOrderWorkflow
    {
        public void Execute()
        {
            IOrderRepository repo = DIContainer.Kernel.Get<IOrderRepository>();
            OrderManager manager = new OrderManager(repo);
            while (true)
            {

                Console.Clear();
                Console.WriteLine("Display Orders");
                Console.WriteLine("--------------------------");
                Console.WriteLine("Enter Q to quit to main menu.");
                Console.WriteLine("Enter an order date to display orders.");
                Console.Write("Separate month, day and year using (/,-,.) characters or a space. Example (5/12/77 or 5 12 77): ");
                string dateString = Console.ReadLine();

                if (dateString.ToUpper() == "Q")
                {
                    return;
                }

                DateTime date = new DateTime();

                LookUpOrderResponse response = new LookUpOrderResponse();

                if (DateTime.TryParse(dateString, out date))
                {
                    response = manager.LookUpOrder(date.ToShortDateString());
                }
                else
                {
                    Console.WriteLine("You must enter a valid date in the proper format!");
                    Console.WriteLine("Separate month, day and year using (/,-,.) characters or a space. Example (5/12/77 or 5 12 77).");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    continue;
                }



                if (response.Success)
                {
                    while (true)
                    {
                        ConsoleIO.DisplayOrderDetails(response.orders);

                        Console.Write("Look up another Order? Y for Yes N to return to main menu: ");
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
                else
                {
                    Console.WriteLine("An error occurred: ");
                    Console.WriteLine(response.Message);
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                    break;
                }
            }
        }
    }
}
