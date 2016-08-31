using System;

namespace Valitru.Sample.Models
{
    public class Order
    {
        public int? OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? ShipDateTime { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public string ConfirmationNumber { get; set; }
        public string ShippingAddressStreet1 { get; set; }
        public string ShippingAddressStreet2 { get; set; }
        public string ShippingAddressCity { get; set; }
    }

    public enum OrderStatuses
    {
        None = 0,
        Placed = 1,
        Processed = 2,
        Shipped = 3
    }
}
