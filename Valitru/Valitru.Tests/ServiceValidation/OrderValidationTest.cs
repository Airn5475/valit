using System;
using System.Linq;
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

        [TestMethod]
        public void RuleCustomersCannotHaveMoreThanFiveOrdersAMonth_CustomerHasAlreadyPlacedThreeOrders_Valid()
        {
            //Arrange
            var order = new Order { OrderId = null, CustomerId = 2, OrderDateTime = DateTime.Today};
            //Act
            var res = _orderValidation.RuleCustomersCannotHaveMoreThanFiveOrdersAMonth().Validate(order);
            //Assert
            Assert.IsTrue(res.IsValid);
        }

        [TestMethod]
        public void RuleCustomersCannotHaveMoreThanFiveOrdersAMonth_CustomerHasAlreadyPlacedFiveOrders_NotValid()
        {
            //Arrange
            var order = new Order { OrderId = null, CustomerId = 1, OrderDateTime = DateTime.Today};
            //Act
            var res = _orderValidation.RuleCustomersCannotHaveMoreThanFiveOrdersAMonth().Validate(order);
            //Assert
            Assert.IsFalse(res.IsValid);
        }

        [TestMethod]
        public void AllRulesStopProcessing_OrderHasDuplicateConfirmationNumber_NoShippingStreetAddressChecked()
        {
            //Arrange
            var order = new Order
            {
                OrderId = 1,
                OrderDateTime = DateTime.Today.AddDays(-3),
                OrderStatus = OrderStatuses.Shipped,
                ShippingAddressStreet1 = null,
                ShipDateTime = DateTime.Today.AddDays(-1),
                ConfirmationNumber = "Duplicate"
            };
            //Act
            var res = _orderValidation.AllRules().Validate(order);
            //Assert
            Assert.IsFalse(res.IsValid);
            Assert.AreEqual(1, res.ValidationResults.Count());
        }

        [TestMethod]
        public void AllRulesStopProcessing_OrderHasDuplicateConfirmationNumber_NoMaxOrdersChecked()
        {
            //Arrange
            var order = new Order
            {
                OrderId = 1,
                CustomerId = 1,
                OrderDateTime = DateTime.Today.AddDays(3),
                OrderStatus = OrderStatuses.Shipped,
                ShippingAddressStreet1 = "206 Riffel",
                ShipDateTime = DateTime.Today.AddDays(5),
                ConfirmationNumber = Guid.NewGuid().ToString()
            };
            //Act
            var res = _orderValidation.AllRules().Validate(order);
            //Assert
            Assert.IsFalse(res.IsValid);
            Assert.AreEqual(1, res.ValidationResults.Count());
        }
    }
}
