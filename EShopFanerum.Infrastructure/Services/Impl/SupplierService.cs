using AutoMapper;
using EShopFanerum.Domain.Entites.Materials;
using EShopFanerum.Infrastructure.Dto.Stock;
using EShopFanerum.Persistance.Repositories.Material;

namespace EShopFanerum.Infrastructure.Services.Impl;

public class SupplierService : GenericService<SupplierDto, Supplier, long>, ISupplierService
{
    private readonly ISupplierRepository _repository;
    public SupplierService(IMapper mapper, ISupplierRepository repository) : base(mapper, repository)
    {
        _repository = repository;
    }
}