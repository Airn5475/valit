using System;

namespace Valitru.Sample.Models
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
