using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Domain.Carrito;
using INCHE.Carrito_Compras.Dtos;
using INCHE.Carrito_Compras.Dtos.Responses;

namespace INCHE.Carrito_Compras.Application.Mappers
{
    public interface ICartMapper
    {
        CartResponse ToResponse(Carrito carrito);
    }

    public class CartMapper : ICartMapper
    {
        private readonly IProductRuleProvider _rules;

        public CartMapper(IProductRuleProvider rules) => _rules = rules;

        public CartResponse ToResponse(Carrito c)
        {
            var rules = _rules.GetDefault();

            decimal CartTotal = 0;

            var resp = new CartResponse { CartId = c.Codigo, Items = new List<CartItemResponse>() };


            foreach (var i in c.Elementos)
            {
                var itemResp = new CartItemResponse
                {
                    ItemId = i.Id,
                    ProductId = i.ProductoId,
                    Quantity = i.Cantidad,
                    BasePrice = rules.BasePrice
                };

                itemResp.BaseSubtotal = itemResp.BasePrice * itemResp.Quantity;

                var selections = new List<SelectionDto>();
                decimal attributesImpact = 0;

                foreach (var s in i.Selecciones)
                {
                    var selResp = new SelectionDto
                    {
                        GroupAttributeId = s.GrupoAtributoId,
                        Attributes = new List<SelectionAttributeDto>()
                    };


                    bool showImpact = rules.Groups.TryGetValue(s.GrupoAtributoId, out var g) && g.ShowPricePerProduct;

                    decimal groupImpact = 0;

                    foreach (var a in s.Atributos)
                    {
                        decimal unitImpact = 0;

                        if (showImpact &&
                            rules.Groups.TryGetValue(s.GrupoAtributoId, out var g2) &&
                            g2.Attributes.TryGetValue(a.AtributoId, out var ra))
                        {
                            unitImpact = ra.PriceImpactAmount;
                        }

                        var attrUnitTotal = unitImpact * a.Cantidad;
                        var attrSubTotal = attrUnitTotal * itemResp.Quantity ;



                        var attrResp = new SelectionAttributeDto
                        {
                            AttributeId = a.AtributoId,
                            Quantity = a.Cantidad,
                            UnitPriceImpact = unitImpact,
                            TotalImpact = unitImpact * a.Cantidad ,
                            TotalImpactSubTotal = attrSubTotal
                        };

                        groupImpact += attrResp.TotalImpact;
                        selResp.Attributes.Add(attrResp);
                    }

                    selResp.GroupImpact = groupImpact;
                    selResp.GroupImpactSubtotal = groupImpact * itemResp.Quantity;

                    attributesImpact += groupImpact;
                    selections.Add(selResp);
                }

                itemResp.AttributesImpact = attributesImpact;
                itemResp.AttributesImpactSubtotal = attributesImpact * itemResp.Quantity;

                itemResp.Total = itemResp.BaseSubtotal + itemResp.AttributesImpactSubtotal;

                ((List<CartItemResponse>)resp.Items).Add(itemResp);
                CartTotal += itemResp.Total;

                itemResp.Selections = selections;
            }

            resp.Total = CartTotal;
            return resp;
        }
    }
}
