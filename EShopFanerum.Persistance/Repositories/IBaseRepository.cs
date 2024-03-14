using EShopFanerum.Domain.Entites;

namespace EShopFanerum.Persistance.Repositories;

/// Интерфейс базового репозитория.
public interface IBaseRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    
    /// Метод получения сущности по id.
    IQueryable<TEntity> GetById(TKey entityId);
    
    /// Метод получение перечня сущностей.
    IQueryable<TEntity> GetEntities();
    
    /// Метод получение перечня сущностей.
    IQueryable<TEntity> GetEntitiesWithPaging(int pageSize, int pageIndex);
    
    /// Метод добавления сущности.
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// Метод удаления сущности.
    TEntity Delete(TKey entityId);
    
    /// Метод обновления сущности.
    TEntity Update(TEntity entity);
    
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}