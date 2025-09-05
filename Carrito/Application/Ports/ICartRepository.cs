

using INCHE.Carrito_Compras.Domain.Carrito;

namespace INCHE.Carrito_Compras.Application.Ports
{
    public interface ICartRepository
    {
        Carrito? Get(string cartCode);
        Carrito Upsert(Carrito cart);
        void RemoveItem(string cartCode, Guid itemId);
    }
}
