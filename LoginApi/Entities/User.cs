using LoginApi.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginApi.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get;set; }= ObjectId.GenerateNewId().ToString();
        public required string Name { get;set; }

        public required string UserName {  get;set; }
        
        public required string Password { get;set; }    

        public Roles Role { get;set; }

        public bool IsActive { get; set; }= false;

        public string UserCode { get; set; } = null!;


        public string StoreCode { get; set; } = null!;

    }
}
