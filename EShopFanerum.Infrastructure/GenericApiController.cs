using EShopFanerum.Infrastructure.Dto;
using EShopFanerum.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.Infrastructure;

public abstract class GenericApiController<TDto> : ControllerBase
    where TDto : class, IDto
{
    private readonly IGenericService<TDto, long> _service;

    protected GenericApiController(IGenericService<TDto, long> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<TDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _service.GetAsync(cancellationToken);
    }

    [HttpPost("paging")]
    public async Task<IEnumerable<TDto>> GetAllWithPage(PagingDto pagingDto, CancellationToken cancellationToken)
    {
        return await _service.GetWithPagingAsync(pagingDto.PageSize, pagingDto.PageIndex, cancellationToken);
    }
    
    [HttpGet("{id}")]
    public async Task<TDto> GetOne(long id, CancellationToken cancellationToken)
    {
       return await _service.GetByIdAsync(id, cancellationToken);
    }


    [HttpPost]
    public async Task<TDto> Create([FromBody] TDto createDto, CancellationToken cancellationToken)
    {
        return await _service.AddAsync(createDto, cancellationToken);
    }

    [HttpPatch("{id}")]
    public async Task<TDto> Update([FromBody] TDto updateDto, CancellationToken cancellationToken)
    {
        return await _service.UpdateAsync(updateDto, cancellationToken);
    }


    [HttpDelete("{id}")]
    public async Task<TDto> Delete(int id, CancellationToken cancellationToken)
    {
        return await _service.DeleteAsync(id, cancellationToken);
    }
}