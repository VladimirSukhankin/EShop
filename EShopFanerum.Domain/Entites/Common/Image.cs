namespace EShopFanerum.Domain.Entites.Common;

public class Image
{
    public long Id { get; set; }

    public ICollection<byte> ImageSrc { get; set; } = new List<byte>();
}