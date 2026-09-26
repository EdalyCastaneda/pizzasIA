namespace HotPizza.Application.Common;

/// <summary>
/// Contenedor de resultado para validaciones de entrada.
/// </summary>
public class ValidationResult
{
    private readonly List<string> _errors = new();

    public bool IsValid => _errors.Count == 0;
    public IReadOnlyList<string> Errors => _errors.AsReadOnly();

    public static ValidationResult Success() => new();

    public static ValidationResult Failure(IEnumerable<string> errors)
    {
        var result = new ValidationResult();
        result._errors.AddRange(errors);
        return result;
    }

    public static ValidationResult Failure(string error)
    {
        var result = new ValidationResult();
        result._errors.Add(error);
        return result;
    }

    public void AddError(string error)
    {
        if (!string.IsNullOrWhiteSpace(error))
        {
            _errors.Add(error);
        }
    }
}
