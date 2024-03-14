namespace EShopFanerum.Infrastructure.Dto.Shop;

public class GoodDto : IDto
{
        public long Id { get; set; }
    
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public decimal Price { get; set; }
}