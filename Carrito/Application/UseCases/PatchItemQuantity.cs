using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Domain.Common;

namespace INCHE.Carrito_Compras.Application.UseCases
{
    public class PatchItemQuantity
    {
        private readonly ICartRepository _repo;
        public PatchItemQuantity(ICartRepository repo) => _repo = repo;

        public Resultado<Carrito> Execute(string cartId, Guid itemId, int delta)
        {
            var cart = _repo.Get(cartId) ?? Carrito.Crear(cartId);
            var item = cart.Obtener(itemId);
            if (item is null) return Resultado<Carrito>.Fail("Item no encontrado.");

            var nueva = item.Cantidad + delta;
            if (nueva <= 0) return Resultado<Carrito>.Fail("La cantidad final no puede ser <= 0.");
            item.CambiarCantidad(nueva);

            _repo.Upsert(cart);
            return Resultado<Carrito>.Ok(cart);
        }
    }
}
