using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using System.Collections.Concurrent;

namespace INCHE.Carrito_Compras.Infraestructure.Persistence
{
    public sealed class InMemoryCartRepository : ICartRepository
    {
        private static readonly ConcurrentDictionary<string, Carrito> Db = new();

        public Carrito? Get(string cartCode) => Db.TryGetValue(cartCode, out var c) ? c : null;

        public Carrito Upsert(Carrito cart)
        {
            Db[cart.Codigo] = cart;
            return cart;
        }

        public void RemoveItem(string cartCode, Guid itemId)
        {
            if (Db.TryGetValue(cartCode, out var c))
            {
                c.Eliminar(itemId);
                Db[cartCode] = c;
            }
        }
    }
}
