using EShopFanerum.Infrastructure.Dto;

namespace EShopFanerum.Infrastructure.Services;

    public interface IGenericService<TDto, TKey>
        where TDto : class, IDto
        where TKey : IEquatable<TKey>
    {
        /// Метод получения по id.
        Task<TDto> GetByIdAsync(TKey dtoId, CancellationToken cancellationToken);
    
        /// Метод получение перечня.
        Task<List<TDto>> GetAsync(CancellationToken cancellationToken);
    
        /// Метод получение перечня c пагинацией.
        Task<List<TDto>> GetWithPagingAsync(int pageSize, int pageIndex, CancellationToken cancellationToken);
    
        /// Метод добавления.
        Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken);
    
        /// Метод удаления.
        Task<TDto> DeleteAsync(TKey dtoId, CancellationToken cancellationToken);
    
        /// Метод обновления.
        Task<TDto> UpdateAsync(TDto dto, CancellationToken cancellationToken);

    }
