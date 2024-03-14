using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.Persistance.Pagination;

public class Pageable : IPageable
{
    [FromQuery(Name = "page")]
    public int PageNumber { get; set; }
    
    [Required]
    [FromQuery(Name = "size")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите значение больше чем {1}")]
    public int PageSize { get; set; }
}