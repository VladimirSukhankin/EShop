namespace EShopFanerum.Infrastructure.Requests.Good;

public class GetGoodByIdsRequest
{
    public List<long> GoodIds { get; set; } = new List<long>();
}