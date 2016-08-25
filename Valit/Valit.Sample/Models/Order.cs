using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valit.Sample.Models
{
    public class Order
    {
        public int? OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? ShipDateTime { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public enum OrderStatuses
    {
        None = 0,
        Placed = 1,
        Processed = 2,
        Shipped = 3
    }
}
