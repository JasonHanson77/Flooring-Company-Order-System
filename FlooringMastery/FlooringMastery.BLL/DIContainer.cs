using FlooringMastery.Data;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class DIContainer
    {
        public static IKernel Kernel = new StandardKernel();

        static DIContainer()
        {
            string mode = ConfigurationManager.AppSettings["appSettings"].ToString();

            if (mode == "Prod")
            {
                Kernel.Bind<IOrderRepository>().To<OrdersProdRepository>().WithConstructorArgument("directoryPath", Settings.DirectoryPath);
                Kernel.Bind<IProductRepository>().To<ProductRepository>().WithConstructorArgument("directoryPath", Settings.DirectoryPath);
                Kernel.Bind<ITaxRepository>().To<TaxInfoRepository>().WithConstructorArgument("directoryPath", Settings.DirectoryPath);
            }
            else if (mode == "Test")
            {
                Kernel.Bind<IOrderRepository>().To<OrdersTestRepository>();
                Kernel.Bind<IProductRepository>().To<ProductsTestRepository>();
                Kernel.Bind<ITaxRepository>().To<TaxInfoTestRepository>();
            }
            else
            {
                throw new Exception("Chooser key in app.config not set properly!");
            }
        }
    }
}
