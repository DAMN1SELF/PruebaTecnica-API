using INCHE.Carrito_Compras.Domain.Common;

namespace INCHE.Carrito_Compras.Domain.Carrito
{
    public sealed class SeleccionGrupo : ValueObject
    {
        public string GrupoAtributoId { get; }
        public IReadOnlyCollection<SeleccionAtributo> Atributos { get; set; }
        public SeleccionGrupo(string grupoAtributoId, IEnumerable<SeleccionAtributo> atributos)
        {
            GrupoAtributoId = grupoAtributoId;
            Atributos = atributos.ToList().AsReadOnly();
        }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return GrupoAtributoId;
            foreach (var a in Atributos.OrderBy(x => x.AtributoId)) yield return a;
        }
    }
}
