using INCHE.Carrito_Compras.Application.Mappers;
using INCHE.Carrito_Compras.Application.UseCases;
using INCHE.Carrito_Compras.Dtos.Requests;
using INCHE.Carrito_Compras.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CarritoCompras.Controllers
{
    public class CartsController : Controller
    {
        private readonly AddItemToCart _addItem; 
        private readonly GetCart _getCart;
        private readonly ICartMapper _mapper;
        public CartsController(
         AddItemToCart addItem, GetCart getCart,
            ICartMapper mapper)
        {
            _addItem = addItem; 
            _mapper = mapper; 
            _getCart = getCart;

        }

        [HttpPost("producto")]
        public ActionResult<CartResponse> AddProducto(string cartId, [FromBody] AddToCartRequest body)
        {
        
            var res = _addItem.Execute(cartId, body) ;
            if (!res.EsExitoso) return UnprocessableEntity(new { error = res.Error });

            var (cart, _) = _getCart.Execute(cartId) ;   
            return Ok(_mapper.ToResponse(cart!));       
        }


  


    }
}
