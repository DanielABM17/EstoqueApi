using System.Text.Json.Serialization;

namespace EstoqueApi.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        
        entregue,
        cancelado,
        problema,
        aguardandoPedido,
        aguardandoLentes,
        aguardandoArmação,
        emProducao
    }
}
