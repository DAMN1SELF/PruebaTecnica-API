using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Domain.Common;
using INCHE.Carrito_Compras.Dtos.Requests;

namespace INCHE.Carrito_Compras.Application.UseCases
{
    public class UpdateItemInCart
    {
        private readonly ICartRepository _repo;
        private readonly IProductRuleProvider _rules;

        public UpdateItemInCart(ICartRepository repo, IProductRuleProvider rules)
        { _repo = repo; _rules = rules; }

        public Resultado<Carrito> Execute(string cartId, Guid itemId, UpdateItemRequest req)
        {
            var cart = _repo.Get(cartId) ?? Carrito.Crear(cartId);
            var item = cart.Obtener(itemId);
            if (item is null) return Resultado<Carrito>.Fail("Item no encontrado.");

            if (req.Quantity.HasValue)
            {
                var q = req.Quantity.Value;
                if (q <= 0) return Resultado<Carrito>.Fail("Cantidad debe ser > 0.");
                item.CambiarCantidad(q);
            }

            var prules = _rules.GetDefault();

            var nuevas = new List<SeleccionGrupo>();
            for (int i = 0; i < req.Selections.Count; i++)
            {
                var grp = req.Selections[i];
                if (!prules.Groups.TryGetValue(grp.GroupAttributeId, out var gr)) continue;
                if (!gr.IsEditable) continue;

                var attrs = new List<SeleccionAtributo>();
                for (int j = 0; j < grp.Attributes.Count; j++)
                {
                    var a = grp.Attributes[j];
                    if (!gr.Attributes.TryGetValue(a.AttributeId, out var ar)) continue;
                    if (!ar.IsEditable) continue;
                    attrs.Add(new SeleccionAtributo(a.AttributeId, a.Quantity));
                }
                if (attrs.Count > 0) nuevas.Add(new SeleccionGrupo(grp.GroupAttributeId, attrs));
            }

            var conservadas = new List<SeleccionGrupo>();
            for (int i = 0; i < item.Selecciones.Count; i++)
            {
                var s = item.Selecciones[i];
                if (prules.Groups.TryGetValue(s.GrupoAtributoId, out var gr) && !gr.IsEditable)
                    conservadas.Add(s);
            }

            item.Selecciones = conservadas.Concat(nuevas).ToList();
            _repo.Upsert(cart);
            return Resultado<Carrito>.Ok(cart);
        }
    }
}