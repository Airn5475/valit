﻿using System;
using System.Linq;
using Valitru.Rules;
using Valitru.Rules.Library;
using Valitru.Sample.Interfacs;
using Valitru.Sample.Models;

namespace Valitru.Sample.Services.Validation
{
    public class OrderValidation : ValidationServiceBase<Order>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderValidation(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public ValidationRule<Order> RuleOrderPlacedDateTimeMustBeInThePast()
            =>
            ValidationRule.NewRule<Order>()
                .ValidIf(order => DateTime.Now >= order.OrderDateTime)
                .SetErrorMessage(order => $"Order has an invalid Date/Time of {order.OrderDateTime }")
                .AddInvalidMember(order => order.OrderDateTime);


        public ValidationRule<Order> RuleOrderCannotHaveAShippedDateLaterThanDatePlaced()
            =>
            ConditionalValidationRule.NewRule<Order>()
                .OnlyCheckIf(order => order.ShipDateTime.HasValue)
                .ValidIf(order => order.ShipDateTime.Value >= order.OrderDateTime)
                .SetErrorMessage(order => $"Order has a Ship Date/Time of {order.ShipDateTime.Value} which is earlier than the Date/Time it was placed of { order.OrderDateTime }")
                .AddInvalidMember(order => order.OrderDateTime)
                .AddInvalidMember(order => order.ShipDateTime);

        public ValidationRule<Order> RuleOrderMarkedAsShippedMustHaveAShippedDate()
            =>
            ConditionalValidationRule.NewRule<Order>()
                .OnlyCheckIf(order => order.OrderStatus == OrderStatuses.Shipped)
                .ValidIf(order => order.ShipDateTime.HasValue)
                .SetErrorMessage("Order is marked as shipped, but has no Ship Date/Time specified")
                .AddInvalidMember(order => order.ShipDateTime);

        public ValidationRule<Order> RuleOrderCannotHaveDuplicateConfirmationNumber()
            =>
            ConditionalValidationRule.NewRule<Order>()
                .OnlyCheckIf(order => !string.IsNullOrWhiteSpace(order.ConfirmationNumber))
                .ValidIf(order =>
                {
                    var orders = _orderRepository.GetOrdersByConfirmationNumber(order.ConfirmationNumber).Where(o => o.OrderId == order.OrderId);
                    return !orders.Any();
                })
                .SetErrorMessage(order => $"The confirmation number '{order.ConfirmationNumber}' is already in use.")
                .AddInvalidMember(order => order.ConfirmationNumber);

        public ValidationRule<Order> RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped()
            =>
            new StringCannotBeNullOrWhitespaceRule<Order>()
                .MemberToValidate(order => order.ShippingAddressStreet1)
                .OnlyCheckIf(order => order.OrderStatus == OrderStatuses.Shipped)
                .SetErrorMessage("ShippingAddress is blank.")
                .AddInvalidMember(order => order.ShippingAddressStreet1);

        public override ValidationRules<Order> AllRules() => new ValidationRules<Order>
        {
            Rules =
            {
                RuleOrderCannotHaveAShippedDateLaterThanDatePlaced(),
                RuleOrderMarkedAsShippedMustHaveAShippedDate(),
                RuleOrderCannotHaveDuplicateConfirmationNumber(),
                RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped()
            }
        };
    }
}
