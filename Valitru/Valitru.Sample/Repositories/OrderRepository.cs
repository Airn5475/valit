using System;
using System.Collections.Generic;
using System.Linq;
using Valitru.Sample.Interfacs;
using Valitru.Sample.Models;

namespace Valitru.Sample.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber)
        {
            return Enumerable.Empty<Order>();
        }

        public void Save(Order orderToBePlaced)
        {
            throw new NotImplementedException();
        }
    }
}
