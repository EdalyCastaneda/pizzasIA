using System.Text.Json.Serialization;

namespace HotPizza.Application.DTOs;

/// <summary>
/// Data Transfer Object (DTO) para representar una pizza a través de los límites arquitectónicos.
/// </summary>
public record PizzaDto
{
    [JsonPropertyName("nombre")]
    public string Nombre { get; init; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; init; } = string.Empty;

    [JsonPropertyName("precio")]
    public decimal Precio { get; init; }

    [JsonPropertyName("tamaño")]
    public string Tamaño { get; init; } = string.Empty;

    [JsonPropertyName("imagen")]
    public string Imagen { get; init; } = string.Empty;
}
