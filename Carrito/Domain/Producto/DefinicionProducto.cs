using INCHE.Carrito_Compras.Domain.Common;
using System.Text.Json.Serialization;

namespace INCHE.Carrito_Compras.Domain.Producto
{
    public class DefinicionProducto : AggregateRoot
    {
        [JsonPropertyName("productId")] public long ProductoId { get; private set; }
        [JsonPropertyName("name")] public string Nombre { get; private set; } = string.Empty;
        [JsonPropertyName("price")] public decimal Precio { get; private set; }

        [JsonPropertyName("groupAttributes")]
        public List<GrupoAtributo> GruposAtributo { get; private set; } = new();

        public GrupoAtributo ObtenerGrupo(string grupoId)
            => GruposAtributo.First(g => g.GrupoAtributoId == grupoId);
    }
}
