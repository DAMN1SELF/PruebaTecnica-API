using INCHE.Carrito_Compras.Domain.Common;

namespace INCHE.Carrito_Compras.Domain.Carrito
{
    public sealed class Carrito : AggregateRoot
    {
        public string Codigo { get; private set; } = Guid.NewGuid().ToString("N");
        private readonly List<ElementoCarrito> _elementos = new();
        public IReadOnlyCollection<ElementoCarrito> Elementos => _elementos.AsReadOnly();

        private Carrito(string codigo) { Codigo = codigo; }
        public static Carrito Crear(string codigo) => new(codigo);

        public ElementoCarrito? Obtener(Guid itemId) => _elementos.FirstOrDefault(x => x.Id == itemId);

        public Resultado<ElementoCarrito> Agregar(long productoId, int cantidad, decimal basePrice, IEnumerable<SeleccionGrupo> selecciones)
        {
            var res = ElementoCarrito.Crear(productoId, cantidad, basePrice, selecciones);
            if (!res.EsExitoso) return Resultado<ElementoCarrito>.Fail(res.Error!);

            var nuevo = res.Valor!;
            var existente = _elementos.FirstOrDefault(x => x.MismaConfiguracionQue(nuevo));
            if (existente is null)
            {
                _elementos.Add(nuevo);
                return Resultado<ElementoCarrito>.Ok(nuevo);
            }
            else
            {
                existente.CambiarCantidad(existente.Cantidad + cantidad);
                return Resultado<ElementoCarrito>.Ok(existente);
            }
        }

        public void Eliminar(Guid itemId) => _elementos.RemoveAll(x => x.Id == itemId);

        public decimal Total(Func<string, long, int, decimal> impacto) =>
            _elementos.Sum(e => e.Total(impacto));
    }
}
