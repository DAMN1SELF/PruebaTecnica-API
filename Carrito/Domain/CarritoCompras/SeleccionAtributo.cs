using INCHE.Carrito_Compras.Domain.Common;

namespace INCHE.Carrito_Compras.Domain.Carrito
{
    public sealed class SeleccionAtributo : ValueObject
    {
        public long AtributoId { get; }
        public int Cantidad { get; }
        public SeleccionAtributo(long atributoId, int cantidad)
        {
            if (cantidad < 0) throw new ArgumentOutOfRangeException(nameof(cantidad));
            AtributoId = atributoId;
            Cantidad = cantidad;
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return AtributoId;
            yield return Cantidad;
        }
    }
}
