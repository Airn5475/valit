using System;
using System.Collections.Generic;
using Valit.Sample.Interfacs;
using Valit.Sample.Models;
using Valit.Sample.Repositories;

namespace Valit.Tests.MockRepositories
{
    internal class OrderRepositoryMock : IOrderRepository
    {
        public IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber)
        {
            yield return new Order() { OrderId = 100, ConfirmationNumber = confirmationNumber };
            yield return new Order() { OrderId = 200, ConfirmationNumber = confirmationNumber };
        }

        public void Save(Order orderToBePlaced)
        {
            Console.WriteLine("DB: I have saved the Order you sent me.");
        }
    }
}