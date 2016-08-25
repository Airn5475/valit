using System.Collections.Generic;
using Valit.Sample.Models;

namespace Valit.Sample.Interfacs
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber);
        void Save(Order orderToBePlaced);
    }
}