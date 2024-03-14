using EShopFanerum.Domain.Entites;
using EShopFanerum.Persistance.Contexts;

namespace EShopFanerum.Persistance.Repositories.Impl;

   /// <summary>
/// Базовый репозиторий
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип ключа сущности</typeparam>
public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly StockDbContext Context;
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BaseRepository{TEntity,TKey}"/>.
    /// </summary>
    protected BaseRepository(StockDbContext context)
    {
        Context = context;
    }
    
    /// Метод получения сущности по id
    public IQueryable<TEntity> GetById(TKey entityId)
    {
        return GetEntities().Where(entity => entity.Id.Equals(entityId));
    }
    
    /// Метод получение перечня сущностей
    public IQueryable<TEntity> GetEntities()
    {
        return Context.Set<TEntity>();
    }
    
    /// Метод получение перечня сущностей с пагинацией
    public IQueryable<TEntity> GetEntitiesWithPaging(int pageSize, int pageIndex)
    {
        return GetEntities()
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }

    /// Метод добавления сущности
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

        return entry.Entity;
    }
    
    /// Метод удаления сущности
    public TEntity Delete(TKey entityId)
    {
        var deletedEntity = Context.Set<TEntity>().FirstOrDefault(entity=>entity.Id.Equals(entityId));
        var entry = Context.Set<TEntity>().Remove(deletedEntity!);

        return entry.Entity;
    }
    
    /// Метод обновления сущности
    public TEntity Update(TEntity entity)
    {
        var entry = Context.Set<TEntity>().Update(entity);
        
        return entry.Entity;
    }

    public virtual void SaveChanges()
    {
        Context.SaveChanges();
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}