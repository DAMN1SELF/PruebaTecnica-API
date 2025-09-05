using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Domain.Common;
using INCHE.Carrito_Compras.Dtos.Requests;

namespace INCHE.Carrito_Compras.Application.UseCases
{
    public class AddItemToCart
    {
        private readonly ICartRepository _repo;
        private readonly IProductRuleProvider _rules;

        public AddItemToCart(ICartRepository repo, IProductRuleProvider rules)
        {
            _repo = repo;
            _rules = rules;
        }

        public Resultado<Carrito> Execute(string cartId, AddToCartRequest req)
        {
            var prules = _rules.Get(req.ProductId) ?? _rules.GetDefault();
            if (req.ProductId != prules.ProductId)
                return Resultado<Carrito_Compras.Domain.Carrito.Carrito>.Fail("ProductId inválido para las reglas.");

            var cart = _repo.Get(cartId) ?? Carrito.Crear(cartId);

            var selecciones = new List<SeleccionGrupo>();
            foreach (var s in req.Selections)
            {
                var attrs = new List<SeleccionAtributo>();
                foreach (var a in s.Attributes)
                    attrs.Add(new SeleccionAtributo(a.AttributeId, a.Quantity));

                selecciones.Add(new SeleccionGrupo(s.GroupAttributeId, attrs));
            }

            var r = cart.Agregar(prules.ProductId, req.Quantity, prules.BasePrice, selecciones);
            if (!r.EsExitoso) return Resultado<Carrito_Compras.Domain.Carrito.Carrito>.Fail(r.Error!);

            _repo.Upsert(cart);
            return Resultado<Carrito_Compras.Domain.Carrito.Carrito>.Ok(cart);
        }
    }
}