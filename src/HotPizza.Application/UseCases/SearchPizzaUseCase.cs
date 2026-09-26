using HotPizza.Application.DTOs;
using HotPizza.Application.Interfaces;
using HotPizza.Application.Mappers;

namespace HotPizza.Application.UseCases;

public interface ISearchPizzaUseCase
{
    Task<IReadOnlyList<PizzaDto>> ExecuteAsync(string query, CancellationToken cancellationToken = default);
}

/// <summary>
/// Caso de uso: Buscar pizzas por coincidencia en nombre o ingredientes de la descripción.
/// </summary>
public class SearchPizzaUseCase : ISearchPizzaUseCase
{
    private readonly IPizzaRepository _repository;

    public SearchPizzaUseCase(IPizzaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PizzaDto>> ExecuteAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            var all = await _repository.GetAllAsync(cancellationToken);
            return PizzaMapper.ToDtoList(all);
        }

        var normalizedQuery = query.Trim().ToLowerInvariant();
        var entities = await _repository.GetAllAsync(cancellationToken);

        var matches = entities.Where(p => 
            p.Nombre.ToLowerInvariant().Contains(normalizedQuery) ||
            p.Descripcion.ToLowerInvariant().Contains(normalizedQuery) ||
            p.Tamaño.ToLowerInvariant().Contains(normalizedQuery));

        return PizzaMapper.ToDtoList(matches);
    }
}
