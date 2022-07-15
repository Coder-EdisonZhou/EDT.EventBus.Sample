using System;

namespace EDT.MSA.Ordering.API.Models
{
    public class OrderVO
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
