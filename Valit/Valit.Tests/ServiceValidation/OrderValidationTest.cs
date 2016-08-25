using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valit.Sample.Interfacs;
using Valit.Sample.Models;
using Valit.Sample.Services.Validation;
using Valit.Tests.MockRepositories;

namespace Valit.Tests.ServiceValidation
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
    }
}
