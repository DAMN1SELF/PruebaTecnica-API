namespace INCHE.Carrito_Compras.Dtos
{
    public class SelectionDto
    {
        public string GroupAttributeId { get; set; } = string.Empty;
        public List<SelectionAttributeDto> Attributes { get; set; } = new();
        public decimal GroupImpact { get; set; }
        public decimal GroupImpactSubtotal { get; set; }
    }

    public class SelectionAttributeDto
    {
        public long AttributeId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceImpact { get; set; }    
        public decimal TotalImpact { get; set; }
        public decimal TotalImpactSubTotal { get; set; }
    }
}
