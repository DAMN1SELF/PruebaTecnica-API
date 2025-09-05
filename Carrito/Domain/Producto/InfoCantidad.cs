using INCHE.Carrito_Compras.Domain.Common;
using System.Text.Json.Serialization;

namespace INCHE.Carrito_Compras.Domain.Producto
{
    public class InfoCantidad : ValueObject
    {
        [JsonPropertyName("groupAttributeQuantity")] public int CantidadGrupoAtributo { get; private set; }
        [JsonPropertyName("showPricePerProduct")] public bool MostrarPrecioPorProducto { get; private set; }
        [JsonPropertyName("isVerified")] public bool EsVerificado { get; private set; }
        [JsonPropertyName("verifyValue")] public string ReglaVerificacion { get; private set; } = "EQUAL_THAN";

        [JsonConstructor]
        public InfoCantidad(int groupAttributeQuantity, bool showPricePerProduct, bool isVerified, string verifyValue)
        { CantidadGrupoAtributo = groupAttributeQuantity; MostrarPrecioPorProducto = showPricePerProduct; EsVerificado = isVerified; ReglaVerificacion = verifyValue; }

        public bool EsObligatorioExacto => ReglaVerificacion.Equals("EQUAL_THAN", StringComparison.OrdinalIgnoreCase);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return CantidadGrupoAtributo; yield return MostrarPrecioPorProducto; yield return EsVerificado; yield return ReglaVerificacion;
        }
    }
}
