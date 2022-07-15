using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EDT.MSA.Stocking.API.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdatedDate { get; set; }
    }
}
