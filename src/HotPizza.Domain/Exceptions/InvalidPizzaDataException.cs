namespace HotPizza.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando los datos de una entidad Pizza son inválidos según las reglas de negocio del dominio.
/// </summary>
public class InvalidPizzaDataException : DomainException
{
    public string PropertyName { get; }

    public InvalidPizzaDataException(string propertyName, string message)
        : base($"[Dominio] La propiedad '{propertyName}' no cumple con las reglas de negocio: {message}")
    {
        PropertyName = propertyName;
    }
}
