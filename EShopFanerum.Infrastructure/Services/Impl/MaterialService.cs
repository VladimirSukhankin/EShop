using AutoMapper;
using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Infrastructure.Dto.Stock;
using EShopFanerum.Persistance.Repositories.Material;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class MaterialService : GenericService<MaterialDto, Material, long>, IMaterialService
{
    private readonly IMaterialRepository _repository;
    
    public MaterialService(IMapper mapper, IMaterialRepository repository) : base(mapper, repository)
    {
        _repository = repository;
    }
}