namespace HotPizza.Application.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se encuentra un elemento en el catálogo.
/// </summary>
public class PizzaNotFoundException : Exception
{
    public PizzaNotFoundException(string name)
        : base($"No se encontró ninguna pizza con el nombre '{name}' en el catálogo.")
    {
    }
}

/// <summary>
/// Excepción lanzada cuando ya existe una pizza registrada con el mismo nombre.
/// </summary>
public class DuplicatePizzaException : Exception
{
    public DuplicatePizzaException(string name)
        : base($"Ya existe una pizza registrada con el nombre '{name}' en el catálogo.")
    {
    }
}
