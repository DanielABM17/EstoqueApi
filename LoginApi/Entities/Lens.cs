using System.ComponentModel.DataAnnotations;
using LoginApi.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginApi.Entities
{
    public class Lens
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [RegularExpression(@"^[-+]?\d+(\.(00|25|50|75))?$", ErrorMessage = "As dioptrias devem ser incrementadas em 0,25 e usar o ponto como separador decimal.")]
        public required double Sphere { get; set; }
        [RegularExpression(@"^[-+]?\d+(\.(00|25|50|75))?$", ErrorMessage = "As dioptrias devem ser incrementadas em 0,25 e usar o ponto como separador decimal.")]
        public required double Cylinder { get; set; }
        public double Axis { get; set; }
        [RegularExpression(@"^[-+]?\d+(\.(00|25|50|75))?$", ErrorMessage = "As dioptrias devem ser incrementadas em 0,25 e usar o ponto como separador decimal.")]
        public double Add { get; set; }

        public required Material Material { get; set; }

        public int Quantidade { get; set; }

        [BsonElement("version")]
        public int version { get; set; }



    }
}
