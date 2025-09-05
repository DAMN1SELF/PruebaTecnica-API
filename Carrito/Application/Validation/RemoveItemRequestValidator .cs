using FluentValidation;
using INCHE.Carrito_Compras.Dtos.Requests;

namespace INCHE.Carrito_Compras.Application.Validation
{
    public sealed class RemoveItemRouteRequestValidator : AbstractValidator<RemoveItemRequest>
    {
        public RemoveItemRouteRequestValidator()
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId es obligatorio.");

            RuleFor(x => x.ItemId)
                .NotEqual(Guid.Empty).WithMessage("ItemId no puede ser Guid.Empty.");
        }
    }
}
