using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginApi.Entities
{
    public class Store
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StoreId { get; set; } = ObjectId.GenerateNewId().ToString();
        public required string Address { get; set; }
        public string StoreCode { get; set; } = null!;

        public ICollection<string> UsersId { get; set; } = new List<string>();  
        public ICollection<string> OrderServices { get; set; } = new List<string>();
    }
}
