namespace INCHE.Carrito_Compras.Dtos.Requests
{
    public sealed class RemoveItemRequest
    {
        public string CartId { get; set; } = default!;
        public Guid ItemId { get; set; }
    }
}
