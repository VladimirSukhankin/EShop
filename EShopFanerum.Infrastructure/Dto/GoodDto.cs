namespace EShopFanerum.Infrastructure.Dto;

public class GoodDto
{
        public long Id { get; set; }
    
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? CategoryName { get; set; }

        public decimal Price { get; set; }
}