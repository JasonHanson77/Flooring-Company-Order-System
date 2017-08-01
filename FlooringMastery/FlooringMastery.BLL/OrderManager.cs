using FlooringMastery.Models;
using FlooringMastery.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.BLL
{
    public class OrderManager
    {
        private IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public LookUpOrderResponse LookUpOrder(string date)
        {
            LookUpOrderResponse response = new LookUpOrderResponse();

            response.orders = _orderRepository.GetOrdersByDate(date);

            if (response.orders.Count() == 0)
            {
                response.Success = false;
                response.Message = $"No orders are available for {date}!";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public void AddOrder(OrderValidationResponse response)
        {
            if (response.Success)
            {
                _orderRepository.SaveOrder(response.order);
            }
            else
            {
                throw new Exception(response.Message);
            }
        }
        public void EditOrder(OrderValidationResponse response)
        {
            if (response.Success)
            {
                _orderRepository.EditOrder(response.order);
            }
            else
            {
                throw new Exception(response.Message);
            }
        }

        public void RemoveOrder(DateTime date, int orderNumber)
        {
            _orderRepository.RemoveOrder(date, orderNumber);
        }
    }
}
