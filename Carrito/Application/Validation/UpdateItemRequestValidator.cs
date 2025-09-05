using FluentValidation;
using FluentValidation.Results;
using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Dtos;
using INCHE.Carrito_Compras.Dtos.Requests;
using INCHE.Carrito_Compras.Dtos.Responses;

namespace INCHE.Carrito_Compras.Application.Validation
{
    public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
    {
        private readonly IProductRuleProvider _rules;

        public UpdateItemRequestValidator(IProductRuleProvider rules)
        {
            _rules = rules;

            RuleFor(x => x.Selections).NotNull();

            RuleFor(x => x.Quantity)
           .GreaterThan(0)
           .When(x => x.Quantity.HasValue);

            RuleFor(x => x).Custom(ValidateImperative);
        }
        private void ValidateImperative(UpdateItemRequest req, ValidationContext<UpdateItemRequest> ctx)
        {
            var prules = _rules.GetDefault();
            if (prules == null) return;

            var selByGroup = new Dictionary<string, List<SelectionAttributeDto>>(StringComparer.OrdinalIgnoreCase);
            if (req.Selections != null)
            {
                for (int i = 0; i < req.Selections.Count; i++)
                {
                    var sel = req.Selections[i];
                    if (!selByGroup.ContainsKey(sel.GroupAttributeId))
                        selByGroup[sel.GroupAttributeId] = new List<SelectionAttributeDto>();

                    for (int j = 0; j < sel.Attributes.Count; j++)
                        selByGroup[sel.GroupAttributeId].Add(sel.Attributes[j]);
                }
            }

            foreach (var gEntry in prules.Groups)
            {
                var g = gEntry.Value;
                if (!g.IsRequired) continue;

                var missing = !selByGroup.ContainsKey(g.GroupAttributeId) || selByGroup[g.GroupAttributeId].Count == 0;
                if (missing)
                {
                    var suggestion = new ValidationSuggestion
                    {
                        GroupAttributeId = g.GroupAttributeId,
                        GroupName = g.GroupName ?? g.GroupAttributeId
                    };

                    foreach (var attr in g.Attributes.Values)
                    {
                        if (!attr.IsActive) continue;
                        suggestion.Options.Add(new ValidationOption
                        {
                            ProductId = attr.ProductId ?? 0,
                            AttributeId = attr.AttributeId,
                            Name = attr.Name ?? attr.AttributeId.ToString(),
                            MaxQuantity = attr.MaxQuantity,
                            PriceImpactAmount = attr.PriceImpactAmount
                        });
                    }

                    var failure = new ValidationFailure(
                        "Grupo " + g.GroupAttributeId,
                        "Grupo obligatorio no enviado."
                    )
                    {
                        ErrorCode = "REQUIRED_GROUP_MISSING",
                        CustomState = suggestion
                    };
                    ctx.AddFailure(failure);
                }
            }

            foreach (var kvp in selByGroup)
            {
                var groupId = kvp.Key;
                var attrsList = kvp.Value;

                if (!prules.Groups.TryGetValue(groupId, out var gr))
                {
                    ctx.AddFailure("Grupo " + groupId, "Grupo no existe en reglas.");
                    continue;
                }

                if (!gr.IsEditable)
                {
                    ctx.AddFailure("Grupo " + groupId, "El grupo no es editable.");
                }

                if (gr.IsVerified)
                {
                    int total = 0;
                    for (int k = 0; k < attrsList.Count; k++)
                        total += attrsList[k].Quantity;

                    if (gr.VerifyValue == VerifyValueRule.EQUAL_THAN && total != gr.GroupAttributeQuantity)
                        ctx.AddFailure("Grupo " + groupId, "Cantidad permitida " + gr.GroupAttributeQuantity + ".");

                    if (gr.VerifyValue == VerifyValueRule.LOWER_EQUAL_THAN && total > gr.GroupAttributeQuantity)
                        ctx.AddFailure("Grupo " + groupId, "Cantidad excede " + gr.GroupAttributeQuantity + ".");
                }

                for (int k = 0; k < attrsList.Count; k++)
                {
                    var a = attrsList[k];

                    if (!gr.Attributes.TryGetValue(a.AttributeId, out var ar))
                    {
                        ctx.AddFailure("Grupo " + groupId, "Atributo " + a.AttributeId + " no existe.");
                        continue;
                    }

                    if (!ar.IsEditable)
                        ctx.AddFailure("Atributo " + a.AttributeId, "El atributo no es editable.");

                    if (!ar.IsActive)
                        ctx.AddFailure("Atributo " + a.AttributeId, "Atributo inactivo.");

                    if (a.Quantity < 0)
                        ctx.AddFailure("Atributo " + a.AttributeId, "la cantidad no puede ser negativa");

                    if (a.Quantity > ar.MaxQuantity)
                        ctx.AddFailure("Atributo " + a.AttributeId, "la cantidad excede lo permitido=" + ar.MaxQuantity + ".");
                }
            }
        }
    }
}
    
