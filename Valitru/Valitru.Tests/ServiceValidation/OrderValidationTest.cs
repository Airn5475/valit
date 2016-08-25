using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valitru.Sample.Interfacs;
using Valitru.Sample.Models;
using Valitru.Sample.Services.Validation;
using Valitru.Tests.MockRepositories;

namespace Valitru.Tests.ServiceValidation
{
    [TestClass]
    public class OrderValidationTest
    {
        private OrderValidation _orderValidation;
        private IOrderRepository _orderRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _orderRepository = new OrderRepositoryMock();
            _orderValidation = new OrderValidation(_orderRepository);
        }

        [TestMethod]
        public void RuleOrderCannotHaveAShippedDateLaterThanDatePlaced_NoShipDate_NotApplicable()
        {
            //Arrange
            var order = new Order {OrderDateTime = DateTime.Today.AddDays(-3)};
            //Act
            var res = _orderValidation.RuleOrderCannotHaveAShippedDateLaterThanDatePlaced().Validate(order);
            //Assert
            Assert.IsTrue(res.NotApplicable);
        }

        [TestMethod]
        public void RuleOrderCannotHaveAShippedDateLaterThanDatePlaced_ShipDateAfterOrderDate_Valid()
        {
            //Arrange
            var order = new Order { OrderDateTime = DateTime.Today.AddDays(-3) };
            order.ShipDateTime = order.OrderDateTime.AddDays(7);
            //Act
            var res = _orderValidation.RuleOrderCannotHaveAShippedDateLaterThanDatePlaced().Validate(order);
            //Assert
            Assert.IsTrue(res.IsValid);
        }

        [TestMethod]
        public void RuleOrderCannotHaveAShippedDateLaterThanDatePlaced_ShipDateBeforeOrderDate_NotValid()
        {
            //Arrange
            var order = new Order { OrderDateTime = DateTime.Today.AddDays(-3) };
            order.ShipDateTime = order.OrderDateTime.AddDays(-7);
            //Act
            var res = _orderValidation.RuleOrderCannotHaveAShippedDateLaterThanDatePlaced().Validate(order);
            //Assert
            Assert.IsFalse(res.IsValid);
        }
        
        [TestMethod]
        public void RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped_ShippedWithStreet1_Valid()
        {
            //Arrange
            var order = new Order { OrderStatus = OrderStatuses.Shipped, ShippingAddressStreet1 = "206 Riffel"};
            //Act
            var res = _orderValidation.RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped().Validate(order);
            //Assert
            Assert.IsTrue(res.IsValid);
        }

        [TestMethod]
        public void RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped_ShippedWithStreet1_NotValid()
        {
            //Arrange
            var order = new Order { OrderStatus = OrderStatuses.Shipped, ShippingAddressStreet1 = null };
            //Act
            var res = _orderValidation.RuleOrderMustHaveAShippingAddressStreet1WhenMarkedAsShipped().Validate(order);
            //Assert
            Assert.IsFalse(res.IsValid);
        }
    }
}
