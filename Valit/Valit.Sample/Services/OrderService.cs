using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valit.Sample.Models;
using Valit.Sample.Repositories;
using Valit.Sample.Services.Validation;

namespace Valit.Sample.Services
{
    public class OrderService
    {
        private readonly OrderValidation _orderValidator;
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _orderValidator = new OrderValidation(_orderRepository);
        }

        public bool PlaceOrder(Order orderToBePlaced)
        {
            var result = _orderValidator.Validate(orderToBePlaced);
            if (!result.IsValid) { return false; }

            //process the order...
            _orderRepository.Save(orderToBePlaced);

            return true;
        }

    }
}
