namespace INCHE.Carrito_Compras.Application.Ports
{
    public enum VerifyValueRule { EQUAL_THAN, LOWER_EQUAL_THAN }

    public sealed record RuleAttribute(
        long AttributeId ,
        int MaxQuantity ,
        decimal PriceImpactAmount,
        bool IsActive,
        bool IsEditable ,
        bool IsShown ,
        string? Name = null,      
        long? ProductId = null
    );

    public sealed record RuleGroup(
        string GroupAttributeId,
        int GroupAttributeQuantity,
        VerifyValueRule VerifyValue,
        bool IsRequired,
        bool IsEditable ,
        bool IsShown,
        bool IsVerified ,
        bool ShowPricePerProduct ,
        IReadOnlyDictionary< long , RuleAttribute> Attributes,
        string? GroupName = null
    );

    public sealed record ProductRules(
        long ProductId,
        decimal BasePrice,
        IReadOnlyDictionary<string, RuleGroup> Groups
    );

    public interface IProductRuleProvider
    {
        ProductRules? Get(long productId);
        ProductRules GetDefault();
        long GetDefaultProductId();
        decimal GetDefaultBasePrice();
    }
}
