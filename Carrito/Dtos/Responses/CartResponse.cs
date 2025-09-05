namespace INCHE.Carrito_Compras.Dtos.Responses
{
    public class CartResponse
    {
        public string CartId { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public IEnumerable<CartItemResponse> Items { get; set; } = Array.Empty<CartItemResponse>();
    }

    public class CartItemResponse
    {
        public Guid ItemId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; } //precio base del producto
        public decimal BaseSubtotal { get; set; } // precio base * cantidad      
        public decimal AttributesImpact { get; set; } //complementos
        public decimal AttributesImpactSubtotal { get; set; } //complementos * cantidad
        public decimal Total { get; set; }           
        public IEnumerable<SelectionDto> Selections { get; set; } = Array.Empty<SelectionDto>();
    }


}
