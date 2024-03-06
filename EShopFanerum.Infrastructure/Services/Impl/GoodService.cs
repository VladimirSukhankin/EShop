using AutoMapper;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Requests;
using EShopFanerum.Infrastructure.Requests.Good;
using EShopFanerum.Persistance.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class GoodService : IGoodService
{
    private readonly IGoodRepository _goodRepository;
    private readonly IMapper _mapper;
    public GoodService(
        IGoodRepository goodRepository, 
        IMapper mapper)
    {
        _goodRepository = goodRepository;
        _mapper = mapper;
    }
    public async Task<ICollection<GoodDto>> GetGoodsWithPagingAsync(PagingDto pagingParams, CancellationToken cancellationToken)
    {
        var goods = await _goodRepository.GetGoodsWithPaging(pagingParams.PageIndex, pagingParams.PageSize).ToListAsync(cancellationToken);
        return _mapper.Map<List<GoodDto>>(goods);
    }

    public async Task AddGoodAsync(AddGoodRequest good, CancellationToken cancellationToken)
    {
        await _goodRepository.AddGoodAsync(_mapper.Map<Good>(good), cancellationToken);
    }

    public Task UpdateGoodAsync(UpdateGoodRequest addGoodRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGoodAsync(long idGood, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}