using System;
using System.Collections.Generic;
using Valitru.Sample.Interfacs;
using Valitru.Sample.Models;

namespace Valitru.Tests.MockRepositories
{
    internal class OrderRepositoryMock : IOrderRepository
    {
        public IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber)
        {
            if (confirmationNumber.ToUpper() == "DUPLICATE")
            {
                yield return new Order() {OrderId = 100, ConfirmationNumber = confirmationNumber};
                yield return new Order() {OrderId = 200, ConfirmationNumber = confirmationNumber};
            }
        }

        public IEnumerable<Order> GetOrdersForCustomerForMonth(int customerId, DateTime forMonth)
        {
            var returnList = new List<Order>();
            var howManyOrders = customerId == 1 ? 5 : 3;
            returnList.AddRange(GenerateOrdersForCustomer(customerId, howManyOrders, forMonth));
            return returnList;
        }

        private IEnumerable<Order> GenerateOrdersForCustomer(int customerId, int numberOfOrders, DateTime ordeDateTime)
        {
            var orderIdBase = customerId * 100;
            for (var i = 1; i <= numberOfOrders; i++)
            {
                yield return new Order() { CustomerId = customerId, OrderId = orderIdBase + i, OrderDateTime = ordeDateTime };
            }
        }

        public void Save(Order orderToBePlaced)
        {
            Console.WriteLine("DB: I have saved the Order you sent me.");
        }
    }
}