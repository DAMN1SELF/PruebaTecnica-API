using INCHE.Carrito_Compras.Domain.Common;
using System.Text.Json.Serialization;

namespace INCHE.Carrito_Compras.Domain.Producto
{
    public class OpcionAtributo : Entity
    {
        [JsonPropertyName("productId")] public long ProductoId { get; private set; }
        [JsonPropertyName("attributeId")] public long AtributoId { get; private set; }
        [JsonPropertyName("name")] public string Nombre { get; private set; } = string.Empty;
        [JsonPropertyName("defaultQuantity")] public int CantidadPorDefecto { get; private set; }
        [JsonPropertyName("maxQuantity")] public int CantidadMaxima { get; private set; }
        [JsonPropertyName("priceImpactAmount")] public decimal MontoImpactoPrecio { get; private set; }
        [JsonPropertyName("isRequired")] public bool EsRequerido { get; private set; }
        [JsonPropertyName("negativeAttributeId")] public string? AtributoNegativoId { get; private set; }
        [JsonPropertyName("statusId")] public string EstadoId { get; private set; } = "A";
    }
}
