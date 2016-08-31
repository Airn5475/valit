using System;
using System.Collections.Generic;
using Valitru.Sample.Models;

namespace Valitru.Sample.Interfacs
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber);
        IEnumerable<Order> GetOrdersForCustomerForMonth(int customerId, DateTime forMonth);
        void Save(Order orderToBePlaced);
    }
}