using HotPizza.Application.DTOs;
using HotPizza.Application.Interfaces;
using HotPizza.Application.Mappers;

namespace HotPizza.Application.UseCases;

public interface IGetPizzaCatalogUseCase
{
    Task<IReadOnlyList<PizzaDto>> ExecuteAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Caso de uso: Consultar catálogo de pizzas.
/// </summary>
public class GetPizzaCatalogUseCase : IGetPizzaCatalogUseCase
{
    private readonly IPizzaRepository _repository;

    public GetPizzaCatalogUseCase(IPizzaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PizzaDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return PizzaMapper.ToDtoList(entities);
    }
}
