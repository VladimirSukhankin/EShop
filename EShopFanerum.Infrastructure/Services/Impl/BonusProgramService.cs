using AutoMapper;
using EShopFanerum.Domain.Entites.Shop;
using EShopFanerum.Infrastructure.Dto.Shop;
using EShopFanerum.Persistance.Repositories;
using EShopFanerum.Persistance.Repositories.Impl.Shop;
using EShopFanerum.Persistance.Repositories.Shop;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class BonusProgramService : GenericService<BonusProgramDto, BonusProgram, long>
{
    private readonly IBonusProgramRepository _repository;
    
    public BonusProgramService(IMapper mapper, IBonusProgramRepository repository) : base(mapper, repository)
    {
        _repository = repository;
    }
}