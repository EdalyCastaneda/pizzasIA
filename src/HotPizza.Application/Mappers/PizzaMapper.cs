using HotPizza.Application.DTOs;
using HotPizza.Domain.Entities;

namespace HotPizza.Application.Mappers;

/// <summary>
/// Mapeador entre la Entidad de Dominio y los DTOs de Aplicación.
/// </summary>
public static class PizzaMapper
{
    public static PizzaDto ToDto(Pizza entity) => new()
    {
        Nombre = entity.Nombre,
        Descripcion = entity.Descripcion,
        Precio = entity.Precio,
        Tamaño = entity.Tamaño,
        Imagen = entity.Imagen
    };

    public static IReadOnlyList<PizzaDto> ToDtoList(IEnumerable<Pizza> entities) =>
        entities.Select(ToDto).ToList().AsReadOnly();

    public static Pizza ToEntity(CreatePizzaRequest request) => new(
        request.Nombre?.Trim() ?? string.Empty,
        request.Descripcion?.Trim() ?? string.Empty,
        request.Precio ?? 0m,
        request.Tamaño?.Trim() ?? string.Empty,
        request.Imagen?.Trim() ?? string.Empty
    );
}
