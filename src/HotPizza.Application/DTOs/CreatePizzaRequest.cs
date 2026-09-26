namespace HotPizza.Application.DTOs;

/// <summary>
/// Modelo de entrada para registrar una nueva pizza en el catálogo.
/// </summary>
public record CreatePizzaRequest(
    string? Nombre,
    string? Descripcion,
    decimal? Precio,
    string? Tamaño,
    string? Imagen
);
