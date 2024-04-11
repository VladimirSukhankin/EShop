using AutoMapper;
using EShopFanerum.Domain.Entites;
using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Infrastructure.Services.Impl;

public abstract class GenericService<TDto, TEntity, TKey> : IGenericService<TDto, TKey>
    where TDto : class, IDto
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly IMapper _mapper;
    private readonly IBaseRepository<TEntity, TKey> _repository;

    protected GenericService(IMapper mapper, IBaseRepository<TEntity, TKey> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<TDto> GetByIdAsync(TKey dtoId, CancellationToken cancellationToken)
    {
        return _mapper.Map<TDto>(await _repository.GetById(dtoId).SingleOrDefaultAsync(cancellationToken));
    }

    public async Task<List<TDto>> GetAsync(CancellationToken cancellationToken)
    {
        return _mapper.Map<List<TDto>>(await _repository.GetEntities().ToListAsync(cancellationToken));
    }

    public async Task<List<TDto>> GetWithPagingAsync(int pageSize, int pageIndex, CancellationToken cancellationToken)
    {
        return _mapper.Map<List<TDto>>(await _repository.GetEntitiesWithPaging(pageSize, pageIndex).ToListAsync(cancellationToken));
    }

    public async Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken)
    {
        var addedDto = _mapper.Map<TDto>(await _repository.AddAsync(_mapper.Map<TEntity>(dto), cancellationToken));
        await _repository.SaveChangesAsync(cancellationToken);
        return addedDto;
    }

    public async Task<TDto> DeleteAsync(TKey dtoId, CancellationToken cancellationToken)
    {
        var deletedDto = _mapper.Map<TDto>(_repository.Delete(dtoId));
        await _repository.SaveChangesAsync(cancellationToken);
        return deletedDto;
    }

    public async Task<TDto> UpdateAsync(TDto dto, CancellationToken cancellationToken)
    {
        var updatedDto = _mapper.Map<TDto>(_repository.Update(_mapper.Map<TEntity>(dto)));
        await _repository.SaveChangesAsync(cancellationToken);
        return updatedDto;
    }
}