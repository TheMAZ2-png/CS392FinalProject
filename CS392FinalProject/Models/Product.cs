using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CS392FinalProject.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("category")]
        public string Category { get; set; } = null!;

        [BsonElement("inStock")]
        public bool InStock { get; set; }

        [BsonElement("tags")]
        public List<string>? Tags { get; set; }

        [BsonElement("specifications")]
        public object? Specifications { get; set; }

        [BsonElement("reviews")]
        public object? Reviews { get; set; }

        [BsonElement("discount")]
        public object? Discount { get; set; }

        [BsonElement("warrantyMonths")]
        public int? WarrantyMonths { get; set; }
    }
}
