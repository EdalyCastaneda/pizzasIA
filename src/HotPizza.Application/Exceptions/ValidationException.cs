namespace HotPizza.Application.Exceptions;

/// <summary>
/// Excepción lanzada cuando los datos de entrada no superan las reglas de validación.
/// </summary>
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Se presentaron uno o más errores de validación de entrada.")
    {
        Errors = errors.ToList().AsReadOnly();
    }

    public ValidationException(string error)
        : base(error)
    {
        Errors = new List<string> { error }.AsReadOnly();
    }
}
