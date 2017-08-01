using FlooringMastery.Data;
using FlooringMastery.Models;
using FlooringMastery.Models.Interfaces;
using FlooringMastery.Models.Responses;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class OrderRules
    {
        ITaxRepository taxRepo = DIContainer.Kernel.Get<ITaxRepository>();
        IProductRepository productRepo = DIContainer.Kernel.Get<IProductRepository>();
        IOrderRepository orderRepo = DIContainer.Kernel.Get<IOrderRepository>();

        public OrderValidationResponse checkOrder(Order newOrder, bool addOrder)
        {
            OrderValidationResponse response = new OrderValidationResponse();
            Order responseOrder = new Order();

            response.order = responseOrder;
            if (addOrder)
            {
                if (newOrder.Date <= DateTime.Today)
                {
                    response.Success = false;
                    response.Message = "You must enter an order date at least one day ahead!";
                    return response;
                }
            }

            if (String.IsNullOrWhiteSpace(newOrder.CustomerName))
            {
                response.Success = false;
                response.Message = "Name field can not be left blank!";
                return response;
            }

            if (!(taxRepo.GetTaxInfo().Any(tax => tax.StateAbbreviation == newOrder.State))
                && !(taxRepo.GetTaxInfo().Any(tax => tax.State == newOrder.State)))
            {
                response.Success = false;
                response.Message = "Sales to this state are prohibited! We can only sell to states on file! Check spelling and try order again!";
                return response;
            }

            if (!productRepo.GetProducts().Any(product => product.ProductType == newOrder.ProductType))
            {
                response.Success = false;
                response.Message = "Product not found in products list! Check spelling and try order again!";
                return response;
            }

            if (newOrder.Area < 100m)
            {
                response.Success = false;
                response.Message = "Minimum order size is 100 square feet!";
                return response;
            }

            response.Success = true;
            response.order.Date = newOrder.Date;
            response.order.CustomerName = newOrder.CustomerName;

            if (addOrder)
            {
                response.order.OrderNumber = GetNewOrderNumber(response.order.Date);
            }
            else
            {
                response.order.OrderNumber = newOrder.OrderNumber;
            }

            response.order.ProductType = newOrder.ProductType;
            response.order.State = newOrder.State;
            response.order.TaxRate = GetTaxRate(response.order.State);
            response.order.Area = newOrder.Area;
            response.order.CostPerSquareFoot = GetProductCost(response.order.ProductType);
            response.order.LaborCostPerSquareFoot = GetLaborCost(response.order.ProductType);
            response.order.LaborCost = response.order.LaborCostPerSquareFoot * response.order.Area;
            response.order.MaterialCost = response.order.CostPerSquareFoot * response.order.Area;
            decimal taxRate = Math.Round(Convert.ToDecimal(response.order.TaxRate), 2) / 100;
            response.order.Tax = Math.Round(Convert.ToDecimal((response.order.LaborCost + response.order.MaterialCost) * taxRate), 2);
            response.order.Total = response.order.LaborCost + response.order.MaterialCost + response.order.Tax;

            return response;

        }

        public decimal GetLaborCost(string productType)
        {
            List<Product> products = productRepo.GetProducts();

            decimal laborCost = 0m;

            foreach (var p in products)
            {
                if (p.ProductType == productType)
                {
                    laborCost = p.LaborCostPerSquareFoot;
                }
            }
            return laborCost;

        }

        public decimal GetProductCost(string productType)
        {
            List<Product> products = productRepo.GetProducts();

            decimal productCost = 0m;

            foreach (var p in products)
            {
                if (p.ProductType == productType)
                {
                    productCost = p.CostPerSquareFoot;
                }
            }
            return productCost;
        }

        public decimal GetTaxRate(string state)
        {
            List<TaxInfo> taxList = taxRepo.GetTaxInfo();

            decimal taxRate = 0m;

            foreach (var t in taxList)
            {
                if (t.State == state || t.StateAbbreviation == state)
                {
                    taxRate = t.TaxRate;
                }
            }
            return taxRate;
        }

        public int GetNewOrderNumber(DateTime date)
        {
            int maxOrderNumber = 0;
            if (orderRepo.GetOrdersByDate(date.ToShortDateString()).Count() > 0)
            {
                maxOrderNumber = orderRepo.GetOrdersByDate(date.ToShortDateString()).Max(order => order.OrderNumber);
            }
            return maxOrderNumber + 1;
        }
    }
}