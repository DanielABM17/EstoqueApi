using System.Text.Json.Serialization;

namespace LoginApi.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]    
    public enum Roles
    {
        Admin,
        Manager,
        Seller,
        Lab,
        Unassigned
        
    }
}
