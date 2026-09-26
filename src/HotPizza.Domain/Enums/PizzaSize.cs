namespace HotPizza.Domain.Enums;

/// <summary>
/// Representa los tamaños estandarizados disponibles para las pizzas.
/// </summary>
public enum PizzaSize
{
    Personal,
    Mediana,
    Grande,
    Familiar
}

public static class PizzaSizeExtensions
{
    public static string ToFriendlyString(this PizzaSize size) => size switch
    {
        PizzaSize.Personal => "Personal",
        PizzaSize.Mediana => "Mediana",
        PizzaSize.Grande => "Grande",
        PizzaSize.Familiar => "Familiar",
        _ => size.ToString()
    };

    public static bool TryParseFriendly(string? input, out PizzaSize size)
    {
        size = PizzaSize.Personal;
        if (string.IsNullOrWhiteSpace(input)) return false;

        return input.Trim().ToLowerInvariant() switch
        {
            "personal" or "1" => SetSize(PizzaSize.Personal, out size),
            "mediana" or "2" => SetSize(PizzaSize.Mediana, out size),
            "grande" or "3" => SetSize(PizzaSize.Grande, out size),
            "familiar" or "4" => SetSize(PizzaSize.Familiar, out size),
            _ => false
        };

        static bool SetSize(PizzaSize target, out PizzaSize outSize)
        {
            outSize = target;
            return true;
        }
    }
}
