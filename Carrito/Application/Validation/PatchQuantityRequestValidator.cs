using FluentValidation;
using INCHE.Carrito_Compras.Dtos.Requests;

namespace INCHE.Carrito_Compras.Application.Validation
{
    public class PatchQuantityRequestValidator : AbstractValidator<PatchQuantityRequest>
    {
        public PatchQuantityRequestValidator()
        {
            RuleFor(x => x.Delta)
                .NotEqual(0).WithMessage("Delta no puede ser 0.")
                .Must(d => Math.Abs(d) <= 10).WithMessage("Delta demasiado grande.");


            RuleFor(x => x.Delta)
                .Must(d => d >= -1 && d <= 10)
                .WithMessage("Delta fuera de rango.");
        }
    }
}
