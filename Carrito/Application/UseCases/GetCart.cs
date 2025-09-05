using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;

namespace INCHE.Carrito_Compras.Application.UseCases
{
    public class GetCart
    {
        private readonly ICartRepository _repo;
        private readonly IProductRuleProvider _rules;
        public GetCart(ICartRepository repo, IProductRuleProvider rules)
        { _repo = repo; _rules = rules; }

        public (Carrito? Cart, ProductRules Rules) Execute(string cartId)
            => (_repo.Get(cartId), _rules.GetDefault());
    }
}
