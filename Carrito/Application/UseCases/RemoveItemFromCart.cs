using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Domain.Common;

namespace INCHE.Carrito_Compras.Application.UseCases
{
    public class RemoveItemFromCart
    {
        private readonly ICartRepository _repo;
        public RemoveItemFromCart(ICartRepository repo) => _repo = repo;

        public Resultado<Carrito> Execute(string cartId, Guid itemId)
        {
            var cart = _repo.Get(cartId) ?? Carrito.Crear(cartId);


            var exists = cart.Elementos.Any(e => e.Id == itemId);
            if (!exists)
                return Resultado<Carrito>.Fail("No existe producto que eliminar.");


            _repo.RemoveItem(cartId, itemId);

            return Resultado<Carrito>.Ok(cart);
        }
    }
}
