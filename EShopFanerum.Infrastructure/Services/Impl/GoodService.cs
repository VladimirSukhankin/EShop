using AutoMapper;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class GoodService : GenericService<GoodDto, Good, long>, IGoodService
{
    private readonly IGoodRepository _goodRepository;
    private readonly IMapper _mapper;
    public GoodService(IMapper mapper, IGoodRepository goodRepository) : base(mapper, goodRepository)
    {
        _mapper = mapper;
        _goodRepository = goodRepository;
    }

    public async Task<List<GoodDto>> GetGoodsByIdsAsync(List<long> ids, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<GoodDto>>(await _goodRepository.GetGoodsByIds(ids).ToListAsync(cancellationToken));
    }
}