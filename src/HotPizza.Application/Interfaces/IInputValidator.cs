using HotPizza.Application.Common;

namespace HotPizza.Application.Interfaces;

/// <summary>
/// Interfaz para validadores de entrada en la capa de Casos de Uso.
/// </summary>
/// <typeparam name="T">Tipo de modelo a validar.</typeparam>
public interface IInputValidator<in T>
{
    ValidationResult Validate(T input);
}
