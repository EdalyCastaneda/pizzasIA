using HotPizza.Domain.Entities;

namespace HotPizza.Application.Interfaces;

/// <summary>
/// Gateway / Puerto de salida para persistencia del catálogo de pizzas (Dependency Inversion Principle).
/// La capa de Casos de Uso define este contrato, que será implementado por los adaptadores de infraestructura.
/// </summary>
public interface IPizzaRepository
{
    Task<IReadOnlyList<Pizza>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Pizza?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Pizza pizza, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ReloadAsync(CancellationToken cancellationToken = default);
    string StorageLocation { get; }
    Task<string> GetRawStorageContentAsync(CancellationToken cancellationToken = default);
}
