using FluentValidation;
using FluentValidation.Results;
using INCHE.Carrito_Compras.Application.Mappers;
using INCHE.Carrito_Compras.Application.UseCases;
using INCHE.Carrito_Compras.Dtos.Requests;
using INCHE.Carrito_Compras.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarritoCompras.Controllers
{
    public class CartsController : Controller
    {
        private readonly AddItemToCart _addItem; 
        private readonly IValidator<AddToCartRequest> _addValidator;

        private readonly GetCart _getCart;
        private readonly ICartMapper _mapper;
        public CartsController(
         AddItemToCart addItem, IValidator<AddToCartRequest> addValidator,
         GetCart getCart,
            ICartMapper mapper
                        )
        {
            _addItem = addItem; 
            _mapper = mapper; 
            _getCart = getCart;
            _addValidator = addValidator;
        }

        [HttpPost("producto")]
        public ActionResult<CartResponse> AddProducto(string cartId, [FromBody] AddToCartRequest body)
        {
            ValidationResult vr = _addValidator.Validate(body);
            if (!vr.IsValid) return BadRequest(ToError(vr));

            var res = _addItem.Execute(cartId, body);
            if (!res.EsExitoso) return UnprocessableEntity(new { error = res.Error });

            var (cart, _) = _getCart.Execute(cartId);
            return Ok(_mapper.ToResponse(cart!));
        }

        private static object ToError(ValidationResult vr)
        {
            var errors = vr.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage));

            var suggestions = new List<ValidationSuggestion>();
            foreach (var e in vr.Errors)
            {
                if (e.CustomState is ValidationSuggestion sug &&
                    !suggestions.Any(s => s.GroupAttributeId == sug.GroupAttributeId))
                {
                    suggestions.Add(sug);
                }
            }

            return new { errors, suggestions };
        }



    }
}
