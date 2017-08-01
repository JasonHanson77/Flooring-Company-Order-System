using FlooringMastery.UI.WorkFlows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.UI
{
    public static class Menu
    {
        public static void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Flooring Mastery");
                Console.WriteLine("------------------------");
                Console.WriteLine("1. Display orders");
                Console.WriteLine("2. Add an order");
                Console.WriteLine("3. Edit an order");
                Console.WriteLine("4. Remove an order");

                Console.WriteLine("\nQ to quit");
                Console.Write("\nEnter selection: ");

                string userinput = Console.ReadLine().ToUpper();

                switch (userinput)
                {
                    case "1":
                        LookUpOrderWorkflow lookupWorkflow = new LookUpOrderWorkflow();
                        lookupWorkflow.Execute();
                        break;
                    case "2":
                        AddOrderWorkflow addOrderWorkflow = new AddOrderWorkflow();
                        addOrderWorkflow.Execute();
                        break;
                    case "3":
                        EditOrderWorkflow editOrderWorkflow = new EditOrderWorkflow();
                        editOrderWorkflow.Execute();
                        break;
                    case "4":
                        RemoveOrderWorkflow removeOrderWorkflow = new RemoveOrderWorkflow();
                        removeOrderWorkflow.Execute();
                        break;
                    case "Q":
                        Environment.Exit(0);
                        return;
                }
            }
        }
    }
}
