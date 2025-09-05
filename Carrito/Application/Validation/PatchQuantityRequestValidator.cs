using FluentValidation;
using INCHE.Carrito_Compras.Dtos.Requests;

namespace INCHE.Carrito_Compras.Application.Validation
{
    public class PatchQuantityRequestValidator : AbstractValidator<PatchQuantityRequest>
    {
        public PatchQuantityRequestValidator()
        {
            RuleFor(x => x.Delta)
                .NotEqual(0).WithMessage("No puede ser 0");


            RuleFor(x => x.Delta)
                .Must(d => d >= -1 && d <= 1)
                .WithMessage("Delta fuera de rango [-1,1].");
        }
    }
}
