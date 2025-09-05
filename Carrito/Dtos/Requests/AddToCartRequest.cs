namespace INCHE.Carrito_Compras.Dtos.Requests
{
    public class AddToCartRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public List<SelectionDto> Selections { get; set; } = new();
    }




}
