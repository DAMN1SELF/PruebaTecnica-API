namespace INCHE.Carrito_Compras.Dtos
{
    public class GroupConfigRoot
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = ""; 
        public decimal Price { get; set; }
        public List<GroupConfig> GroupAttributes { get; set; } = new();
    }

    public class GroupConfig
    {
        public string GroupAttributeId { get; set; } = "";
        public GroupAttributeTypeConfig GroupAttributeType { get; set; } = new();
        public string? Description { get; set; }
        public QuantityInformationConfig QuantityInformation { get; set; } = new();
        public List<AttributeConfig> Attributes { get; set; } = new();
        public int Order { get; set; }
    }

    public class GroupAttributeTypeConfig
    {
        public string GroupAttributeTypeId { get; set; } = "";
        public string Name { get; set; } = "";
    }

    public class QuantityInformationConfig
    {
        public int GroupAttributeQuantity { get; set; }
        public bool ShowPricePerProduct { get; set; }
        public bool IsShown { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVerified { get; set; }
        public string VerifyValue { get; set; } // EQUAL_THAN | LOWER_EQUAL_THAN
    }

    public class AttributeConfig
    {
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public string Name { get; set; } = "";
        public int DefaultQuantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal PriceImpactAmount { get; set; }
        public bool IsRequired { get; set; }
        public string? NegativeAttributeId { get; set; }
        public int Order { get; set; }
        public required string StatusId { get; set; } // A=activo, I=inactivo
        public string? UrlImage { get; set; }
    }
}
