using System.Globalization;
using HotPizza.Application.DTOs;
using HotPizza.Domain.Enums;

namespace HotPizza.UI;

/// <summary>
/// Lector y validador de entrada interactiva de consola.
/// </summary>
public static class ConsoleInputReader
{
    public static CreatePizzaRequest PromptForNewPizza()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n--- Formulario de Registro de Nueva Pizza ---");
        Console.ResetColor();

        // 1. Nombre
        string nombre = ReadRequiredString(
            "1. Ingrese el Nombre de la pizza (ej. Pizza Margarita Especial):",
            "El nombre es obligatorio y debe tener al menos 3 caracteres.",
            minLength: 3,
            maxLength: 80
        );

        // 2. Descripción
        string descripcion = ReadRequiredString(
            "2. Ingrese la Descripción e ingredientes (ej. Salsa napolitana, mozzarella fresca y albahaca):",
            "La descripción es obligatoria y debe tener al menos 10 caracteres.",
            minLength: 10,
            maxLength: 300
        );

        // 3. Precio
        decimal precio = ReadPositiveDecimal(
            "3. Ingrese el Precio en MXN (ej. 159.50):",
            "Debe ingresar un monto numérico positivo válido."
        );

        // 4. Tamaño
        string tamaño = ReadPizzaSize();

        // 5. Imagen
        string imagen = ReadImage(nombre);

        return new CreatePizzaRequest(nombre, descripcion, precio, tamaño, imagen);
    }

    public static string ReadSearchQuery()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Ingrese el nombre, ingrediente o tamaño a buscar: ");
        Console.ResetColor();
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private static string ReadRequiredString(string prompt, string errorMessage, int minLength = 1, int maxLength = 250)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(prompt);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> ");
            Console.ResetColor();

            string? input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input) && input.Length >= minLength && input.Length <= maxLength)
            {
                return input;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Entrada Inválida] {errorMessage} (Longitud permitida: {minLength}-{maxLength} caracteres)");
            Console.ResetColor();
        }
    }

    private static decimal ReadPositiveDecimal(string prompt, string errorMessage)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(prompt);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("> ");
            Console.ResetColor();

            string? input = Console.ReadLine()?.Trim().Replace(',', '.');
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val) && val > 0 && val <= 10000)
            {
                return val;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Entrada Inválida] {errorMessage} Debe ser mayor a 0 y menor a $10,000.00.");
            Console.ResetColor();
        }
    }

    private static string ReadPizzaSize()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("4. Seleccione el Tamaño:");
            Console.WriteLine("   [1] Personal");
            Console.WriteLine("   [2] Mediana");
            Console.WriteLine("   [3] Grande");
            Console.WriteLine("   [4] Familiar");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Seleccione una opción [1-4 o escriba el nombre]: ");
            Console.ResetColor();

            string? input = Console.ReadLine()?.Trim();
            if (PizzaSizeExtensions.TryParseFriendly(input, out var size))
            {
                return size.ToFriendlyString();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Entrada Inválida] Por favor seleccione 1, 2, 3 o 4, o escriba Personal, Mediana, Grande o Familiar.");
            Console.ResetColor();
        }
    }

    private static string ReadImage(string pizzaName)
    {
        string slug = pizzaName.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u")
            .Replace("ñ", "n");

        string suggestedUrl = $"https://hotpizza.com/images/{slug}.jpg";

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"5. Ingrese la URL o ruta de la Imagen [Enter para usar sugerida: {suggestedUrl}]:");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("> ");
        Console.ResetColor();

        string? input = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(input))
        {
            return suggestedUrl;
        }

        return input;
    }
}
