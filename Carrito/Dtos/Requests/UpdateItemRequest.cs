namespace INCHE.Carrito_Compras.Dtos.Requests
{
    public class UpdateItemRequest
    {
        public int? Quantity { get; set; }
        public List<SelectionDto> Selections { get; set; } = new();
    }
}
