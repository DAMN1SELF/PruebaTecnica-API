using FluentValidation;
using FluentValidation.Results;
using INCHE.Carrito_Compras.Application.Mappers;
using INCHE.Carrito_Compras.Application.UseCases;
using INCHE.Carrito_Compras.Dtos.Requests;
using INCHE.Carrito_Compras.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarritoCompras.Controllers
{
    [ApiController]
    [Route("api/carrito/{cartId}")]
    public class CartsController : Controller
    {
        private readonly AddItemToCart _addItem; 
        private readonly IValidator<AddToCartRequest> _addValidator;

        private readonly UpdateItemInCart _updateItem;
        private readonly IValidator<UpdateItemRequest> _updateValidator;
        private readonly GetCart _getCart;
        private readonly ICartMapper _mapper;

        private readonly PatchItemQuantity _patchQuantity;
        private readonly IValidator<PatchQuantityRequest> _patchValidator;

        private readonly RemoveItemFromCart _removeItem;

        private readonly IValidator<RemoveItemRequest> _deleteValidator;

        public CartsController(
         AddItemToCart addItem, IValidator<AddToCartRequest> addValidator,
         UpdateItemInCart updateItem, IValidator<UpdateItemRequest> updateValidator,
         PatchItemQuantity patchQuantity,IValidator<PatchQuantityRequest> patchValidator,
           RemoveItemFromCart removeItem, IValidator<RemoveItemRequest> deleteValidator,
         GetCart getCart,
         ICartMapper mapper
        )
        {
            _addItem = addItem; 
            _mapper = mapper; 
            _getCart = getCart;
            _addValidator = addValidator; 
            _updateItem = updateItem; 
            _updateValidator = updateValidator; 
            _patchQuantity = patchQuantity;
            _patchValidator = patchValidator;
            _removeItem = removeItem; 
            _deleteValidator = deleteValidator ;

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

        [HttpPut("producto/{itemId:guid}")]
        public ActionResult<CartResponse> UpdateItem(string cartId, Guid itemId, [FromBody] UpdateItemRequest body)
        {
            ValidationResult vr = _updateValidator.Validate(body);
            if (!vr.IsValid) return BadRequest(ToError(vr));

            var res = _updateItem.Execute(cartId, itemId, body);
            if (!res.EsExitoso) return UnprocessableEntity(new { error = res.Error });

            var (cart, _) = _getCart.Execute(cartId);
            return Ok(_mapper.ToResponse(cart!));
        }

        [HttpPatch("producto/{itemId:guid}")]
        public ActionResult<CartResponse> PatchQuantity(string cartId, Guid itemId, [FromBody] PatchQuantityRequest body)
        {
            ValidationResult vr = _patchValidator.Validate(body);
            if (!vr.IsValid) return BadRequest(ToError(vr));

            var res = _patchQuantity.Execute(cartId, itemId, body.Delta);
            if (!res.EsExitoso) return UnprocessableEntity(new { error = res.Error });

            var (cart, _) = _getCart.Execute(cartId);
            return Ok(_mapper.ToResponse(cart!));
        }

        [HttpDelete("producto/{itemId:guid}")]
        public ActionResult<CartResponse> DeleteItem(string cartId, Guid itemId)
        {

            var route = new RemoveItemRequest { CartId = cartId, ItemId = itemId };

            var vr = _deleteValidator.Validate(route);
            if (!vr.IsValid) return BadRequest(ToError(vr));

            var res = _removeItem.Execute(route.CartId, route.ItemId);
            if (!res.EsExitoso) return NotFound(new { error = res.Error });

            var (cart, _) = _getCart.Execute(route.CartId);
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
