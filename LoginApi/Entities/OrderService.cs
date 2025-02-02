using EstoqueApi.Entities.Enums;
using LoginApi.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginApi.Entities
{
    public class OrderService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public int OrderNumber { get; set; }
        public required string Description { get; set; } 
        public Lens? RightLens { get; set; }
        public Lens? LeftLens { get; set; }
        public Material Material { get; set; }

        public Status Status { get; set; }

        public Local Local { get; set; }    

        public required DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public required bool Stock {  get; set; } 
        public required string StoreCode { get; set; } 

        public required string UserCode { get; set; } 
    }
}
