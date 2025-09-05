using INCHE.Carrito_Compras.Application.Ports;
using INCHE.Carrito_Compras.Dtos;

namespace INCHE.Carrito_Compras.Infraestructure.Rules
{
    public sealed class ProductRuleProviderFromConfig : IProductRuleProvider
    {
        private readonly Dictionary<long, ProductRules> _byProduct = new();
        private readonly long _defaultProductId;
        private readonly decimal _defaultBasePrice;

        public ProductRuleProviderFromConfig(GroupConfigRoot cfg)
        {
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));
            _defaultProductId = cfg.ProductId;
            _defaultBasePrice = cfg.Price;

            var groups = new Dictionary<string, RuleGroup>(StringComparer.OrdinalIgnoreCase);

            foreach (var g in cfg.GroupAttributes)
            {
                var verify = ParseVerify(g.QuantityInformation.VerifyValue);

                bool isRequired = verify == VerifyValueRule.EQUAL_THAN && g.QuantityInformation.GroupAttributeQuantity >= 1;

                var attrs = new Dictionary<long, RuleAttribute>();
                foreach (var a in g.Attributes)
                {
                    bool isActive = string.Equals(a.StatusId, "A", StringComparison.OrdinalIgnoreCase);
                    attrs[a.AttributeId] = new RuleAttribute(
                        a.AttributeId ,
                        a.MaxQuantity,
                         a.PriceImpactAmount ,
                        isActive ,
                         g.QuantityInformation.IsEditable,
                        g.QuantityInformation.IsShown ,
                         Name: a.Name,              
                        ProductId: a.ProductId     
                    );
                }

                groups[g.GroupAttributeId] = new RuleGroup(

                    GroupAttributeId: g.GroupAttributeId ,
                    GroupAttributeQuantity: g.QuantityInformation.GroupAttributeQuantity,
                    VerifyValue : verify ,
                    IsRequired : isRequired,
                    IsEditable : g.QuantityInformation.IsEditable,
                    IsShown : g.QuantityInformation.IsShown ,
                    IsVerified : g.QuantityInformation.IsVerified  ,
                    ShowPricePerProduct : g.QuantityInformation.ShowPricePerProduct ,
                    Attributes : attrs , 
                    GroupName : g.GroupAttributeType?.Name

                );
            }

            _byProduct[_defaultProductId] = new ProductRules(_defaultProductId, _defaultBasePrice, groups);
        }

        public ProductRules? Get(long productId) => _byProduct.TryGetValue(productId, out var r) ? r : null;

        public ProductRules GetDefault() => _byProduct[_defaultProductId];
        public long GetDefaultProductId() => _defaultProductId;
        public decimal GetDefaultBasePrice() => _defaultBasePrice;

        private static VerifyValueRule ParseVerify(string v) =>
            (v ?? "").Trim().ToUpperInvariant() switch
            {
                "EQUAL_THAN" => VerifyValueRule.EQUAL_THAN,
                "LOWER_EQUAL_THAN" => VerifyValueRule.LOWER_EQUAL_THAN,
                _ => throw new NotSupportedException($"verifyValue '{v}' no soportado")
            };
    }
}