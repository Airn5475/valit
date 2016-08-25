using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valit.Sample.Models;
using Valit.Sample.Services;

namespace Valit.Sample.Repositories
{
    public class OrderRepository
    {
        public IEnumerable<Order> GetOrdersByConfirmatiaonNumber(string confirmationNumber)
        {
            return Enumerable.Empty<Order>();
        }

        public void Save(Order orderToBePlaced)
        {
            throw new NotImplementedException();
        }
    }
}
