using System.Text.Json.Serialization;

namespace LoginApi.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Material
    {
        Resina,
        Policarbonato,
        HighIndex160,
        HighIndex167,
        HighIndex174,
    }
}
