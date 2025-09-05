using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Domain.Common;
using System.Text.Json.Serialization;

namespace INCHE.Carrito_Compras.Domain.Producto
{
    public class GrupoAtributo : Entity
    {
        [JsonPropertyName("groupAttributeId")] public string GrupoAtributoId { get; private set; } = string.Empty;
        [JsonPropertyName("description")] public string? Descripcion { get; private set; }
        [JsonPropertyName("quantityInformation")] public InfoCantidad InfoCantidad { get; private set; } = null!;
        [JsonPropertyName("attributes")] public List<OpcionAtributo> Atributos { get; private set; } = new();
        [JsonPropertyName("order")] public int Orden { get; private set; }

        public Resultado ValidarSelecciones(IReadOnlyCollection<SeleccionAtributo> seleccion)
        {
            var total = seleccion.Sum(s => s.Cantidad);

            if (InfoCantidad.EsObligatorioExacto)
            {
                if (total != InfoCantidad.CantidadGrupoAtributo)
                    return Resultado.Fail($"El grupo {GrupoAtributoId} requiere EXACTA = {InfoCantidad.CantidadGrupoAtributo}.");
            }
            else if (total > InfoCantidad.CantidadGrupoAtributo)
                return Resultado.Fail($"El grupo {GrupoAtributoId} supera la cantidad máxima {InfoCantidad.CantidadGrupoAtributo}.");

            var mapa = Atributos.ToDictionary(a => a.AtributoId);
            foreach (var s in seleccion)
            {
                if (!mapa.TryGetValue(s.AtributoId, out var def))
                    return Resultado.Fail($"Atributo {s.AtributoId} no existe en grupo {GrupoAtributoId}.");

                if (s.Cantidad < 0) return Resultado.Fail("Cantidad de atributo no puede ser negativa.");
                if (s.Cantidad > def.CantidadMaxima)
                    return Resultado.Fail($"Atributo {s.AtributoId} excede MaxQuantity={def.CantidadMaxima}.");

                if (!string.IsNullOrWhiteSpace(def.AtributoNegativoId))
                {
                    var neg = seleccion.Any(x => x.AtributoId.ToString() == def.AtributoNegativoId && x.Cantidad > 0);
                    if (neg && s.Cantidad > 0)
                        return Resultado.Fail($"Atributo {s.AtributoId} es excluyente con {def.AtributoNegativoId}.");
                }
            }

            var reqs = Atributos.Where(a => a.EsRequerido).Select(a => a.AtributoId).ToHashSet();
            foreach (var req in reqs)
            {
                var found = seleccion.FirstOrDefault(x => x.AtributoId == req);
                if (found is null || found.Cantidad <= 0)
                    return Resultado.Fail($"Atributo requerido {req} del grupo {GrupoAtributoId} no fue seleccionado.");
            }

            return Resultado.Ok();
        }

        public decimal CalcularImpacto(IReadOnlyCollection<SeleccionAtributo> seleccion)
        {
            var mapa = Atributos.ToDictionary(a => a.AtributoId);
            return seleccion.Sum(s => mapa[s.AtributoId].MontoImpactoPrecio * s.Cantidad);
        }
    }
}
