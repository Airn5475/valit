using Valitru.Sample.Models;
using Valitru.Sample.Repositories;
using Valitru.Sample.Services.Validation;

namespace Valitru.Sample.Services
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
