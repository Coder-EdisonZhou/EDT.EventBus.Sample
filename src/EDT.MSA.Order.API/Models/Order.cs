using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EDT.MSA.Ordering.API.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Failed = -1,
        Pending = 0,
        Succeed = 1
    }
}
