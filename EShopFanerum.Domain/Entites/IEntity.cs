namespace EShopFanerum.Domain.Entites;

public interface IEntity<TKey>
{ 
    public TKey Id { get; }
}