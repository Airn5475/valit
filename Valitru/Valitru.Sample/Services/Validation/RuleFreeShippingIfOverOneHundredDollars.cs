using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valitru.Helpers;
using Valitru.Rules;
using Valitru.Sample.Interfacs;
using Valitru.Sample.Models;

namespace Valitru.Sample.Services.Validation
{
    public class RuleFreeShippingIfOverOneHundredDollars<T> : CustomValidationRuleBase<T, bool>
    {
        private readonly IOrderRepository _orderRepository;

        protected Func<T, bool> FreeShipping { get; set; }
        protected Func<T, int> CustomerId { get; set; }
        protected Func<T, DateTime> OrderDateTime { get; set; }

        public RuleFreeShippingIfOverOneHundredDollars(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public RuleFreeShippingIfOverOneHundredDollars<T> SetParameters(Func<T, bool> freeShipping, Func<T, int> customerId, Func<T, DateTime> orderDateTime)
        {
            FreeShipping = freeShipping;
            CustomerId = customerId;
            OrderDateTime = orderDateTime;
            return this;
        }

        public override ValidationRuleResult Validate(T instance)
        {
            var monthlyOrders = _orderRepository.GetOrdersForCustomerForMonth(CustomerId(instance), OrderDateTime(instance)).ToList();
            if (ValidatingRequestAction != null)
            {
                Validating(monthlyOrders);
            }
            var monthlyTotal = monthlyOrders.Sum(s => s.SubTotal);

            if (!FreeShipping(instance) || (FreeShipping(instance) && monthlyTotal >= (decimal) 100.00))
            {
                return ValidationRuleResult.ValidationPassedResult();
            }

            ValidIf(o => FreeShipping(instance) && monthlyTotal >= (decimal)100.00);

            return new ValidationRuleResult
            {
                IsValid = false,
                ValidationResults = ValidationResultHelper.NewResult("Free shipping not valid.")
            };
        }

        protected Action<List<Order>> ValidatingRequestAction { get; set; }
        private void Validating(List<Order> ordersToBeProcessed)
        {
            ValidatingRequestAction(ordersToBeProcessed);
        }
    }
}
