namespace INCHE.Carrito_Compras.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
    public abstract class AggregateRoot : Entity { }

    public abstract class ValueObject
    {
        protected abstract IEnumerable<object?> GetEqualityComponents();
        public override bool Equals(object? obj) =>
            obj is ValueObject other && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        public override int GetHashCode() =>
            GetEqualityComponents().Aggregate(1, (h, o) => HashCode.Combine(h, o));
    }

    public class Resultado
    {
        public bool EsExitoso { get; }
        public string? Error { get; }
        protected Resultado(bool ok, string? e) { EsExitoso = ok; Error = e; }
        public static Resultado Ok() => new(true, null);
        public static Resultado Fail(string e) => new(false, e);
    }
    public sealed class Resultado<T> : Resultado
    {
        public T? Valor { get; }
        private Resultado(bool ok, string? e, T? v) : base(ok, e) { Valor = v; }
        public static Resultado<T> Ok(T v) => new(true, null, v);
        public static new Resultado<T> Fail(string e) => new(false, e, default);
    }
}
