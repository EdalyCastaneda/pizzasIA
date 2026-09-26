namespace HotPizza.Domain.Exceptions;

/// <summary>
/// Excepción base para violaciones de reglas de negocio en la capa de dominio (Enterprise Business Rules).
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
