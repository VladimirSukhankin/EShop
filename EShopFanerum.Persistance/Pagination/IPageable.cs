namespace EShopFanerum.Persistance.Pagination;

/// Параметры постраничного вывода
public interface IPageable
{
    /// Номер страницы, нумерация начинается с 0
    int PageNumber { get; }
    
    /// Размер страницы, должен быть больше 0
    int PageSize { get; }
}