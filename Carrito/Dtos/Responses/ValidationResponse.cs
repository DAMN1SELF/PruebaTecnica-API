namespace INCHE.Carrito_Compras.Dtos.Responses
{
    public sealed class ValidationSuggestion
    {
        public string GroupAttributeId { get; set; } = "";
        public string GroupName { get; set; } = "";
        public List<ValidationOption> Options { get; set; } = new();
    }

    public sealed class ValidationOption
    {
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public string Name { get; set; } = "";
        public int MaxQuantity { get; set; }
        public decimal PriceImpactAmount { get; set; }
    }
}
