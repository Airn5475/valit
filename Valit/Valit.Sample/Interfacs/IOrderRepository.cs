using System.Collections.Generic;
using Valitru.Sample.Models;

namespace Valitru.Sample.Interfacs
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrdersByConfirmationNumber(string confirmationNumber);
        void Save(Order orderToBePlaced);
    }
}