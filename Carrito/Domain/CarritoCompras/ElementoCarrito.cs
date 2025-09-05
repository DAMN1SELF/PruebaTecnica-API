using INCHE.Carrito_Compras.Domain.Common;
using INCHE.Carrito_Compras.Domain.Producto;

namespace INCHE.Carrito_Compras.Domain.Carrito
{
    public sealed class ElementoCarrito : Common.Entity
    {
        public long ProductoId { get; private set; }
        public int Cantidad { get; private set; }
        public decimal BasePrice { get; private set; }
        public List<SeleccionGrupo> Selecciones { get; set; } = new();

        private ElementoCarrito() { }

        public static Resultado<ElementoCarrito> Crear(long productoId, int cantidad, decimal basePrice, IEnumerable<SeleccionGrupo> selecciones)
        {
            if (cantidad <= 0) return Resultado<ElementoCarrito>.Fail("Cantidad debe ser > 0.");
            var e = new ElementoCarrito
            {
                ProductoId = productoId,
                Cantidad = cantidad,
                BasePrice = basePrice,
                Selecciones = selecciones.ToList()
            };
            return Resultado<ElementoCarrito>.Ok(e);
        }

        public void CambiarCantidad(int nueva)
        {
            if (nueva <= 0) throw new InvalidOperationException("Cantidad debe ser > 0.");
            Cantidad = nueva;
        }

        public decimal Total(Func<string, long, int, decimal> impacto)
        {
            decimal sum = 0m;
            foreach (var g in Selecciones)
                foreach (var a in g.Atributos)
                    sum += impacto(g.GrupoAtributoId, a.AtributoId, a.Cantidad);
            return (BasePrice + sum) * Cantidad;
        }

        public bool MismaConfiguracionQue(ElementoCarrito otro)
        {
            if (ProductoId != otro.ProductoId) return false;
            string S(IEnumerable<SeleccionGrupo> sels) => string.Join("|", sels.OrderBy(s => s.GrupoAtributoId)
                .Select(s => $"{s.GrupoAtributoId}:{string.Join(",", s.Atributos.OrderBy(x => x.AtributoId).Select(x => $"{x.AtributoId}x{x.Cantidad}"))}"));
            return S(Selecciones) == S(otro.Selecciones);
        }
    }
}
