using HotPizza.Application.DTOs;

namespace HotPizza.Presentation.Presenters;

/// <summary>
/// Implementación del Presenter para la Interfaz de Consola.
/// </summary>
public class ConsolePizzaPresenter : IPizzaPresenter
{
    public void PresentWelcome(string storageLocation)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                                                               ║
║             🍕  H O T   P I Z Z A   C . A .  🍕              ║
║         Clean Architecture System - Uncle Bob Pattern         ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[Sistema] Almacenamiento activo: {storageLocation}");
        Console.WriteLine("[Arquitectura] Capas: Entities -> Use Cases -> Interface Adapters -> Frameworks & Drivers");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void PresentMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                     MENÚ PRINCIPAL                            ║");
        Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║  1. 📋 Ver Catálogo Completo de Pizzas                        ║");
        Console.WriteLine("║  2. ➕ Registrar Nueva Pizza (con Validación)                 ║");
        Console.WriteLine("║  3. 🔍 Buscar Pizza por Nombre o Ingrediente                  ║");
        Console.WriteLine("║  4. 💾 Guardar Catálogo en hotPizza.json                      ║");
        Console.WriteLine("║  5. 🔄 Recargar Catálogo desde hotPizza.json                  ║");
        Console.WriteLine("║  6. 📄 Visualizar Contenido del Archivo JSON                  ║");
        Console.WriteLine("║  7. 🚪 Salir                                                  ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("Seleccione una opción [1-7]: ");
        Console.ResetColor();
    }

    public void PresentCatalog(IReadOnlyList<PizzaDto> pizzas)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.WriteLine($"🍕 CATÁLOGO DE PIZZAS ({pizzas.Count} variedades registradas)");
        Console.WriteLine("═══════════════════════════════════════════════════════════════════════════════════════════════════════════");
        Console.ResetColor();

        if (pizzas.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("El catálogo se encuentra actualmente vacío.");
            Console.ResetColor();
            return;
        }

        int index = 1;
        foreach (var pizza in pizzas)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"#{index:D2} ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{pizza.Nombre.PadRight(30)}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"[{pizza.Tamaño.PadRight(8)}] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"${pizza.Precio,8:N2} MXN");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"    📝 Descripción: {pizza.Descripcion}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    🖼️  Imagen:      {pizza.Imagen}");
            Console.ResetColor();
            Console.WriteLine(new string('-', 107));
            index++;
        }
    }

    public void PresentPizzaDetail(PizzaDto pizza)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n--- Detalle de la Pizza ---");
        Console.WriteLine($"Nombre:      {pizza.Nombre}");
        Console.WriteLine($"Descripción: {pizza.Descripcion}");
        Console.WriteLine($"Precio:      ${pizza.Precio:N2} MXN");
        Console.WriteLine($"Tamaño:      {pizza.Tamaño}");
        Console.WriteLine($"Imagen:      {pizza.Imagen}");
        Console.ResetColor();
    }

    public void PresentSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[✓ ÉXITO] {message}");
        Console.ResetColor();
    }

    public void PresentWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[⚠ ATENCIÓN] {message}");
        Console.ResetColor();
    }

    public void PresentError(string message, IEnumerable<string>? details = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n[✗ ERROR] {message}");
        if (details != null)
        {
            foreach (var detail in details)
            {
                Console.WriteLine($"   • {detail}");
            }
        }
        Console.ResetColor();
    }

    public void PresentJsonContent(string json, string filePath)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"\n📄 Contenido directo de '{filePath}':");
        Console.WriteLine("------------------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(json);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("------------------------------------------------------------");
        Console.ResetColor();
    }

    public void PresentGoodbye()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n¡Gracias por utilizar HotPizza Clean Architecture! Hasta pronto. 🍕");
        Console.ResetColor();
    }

    public void PresentDivider()
    {
        Console.WriteLine(new string('=', 65));
    }

    public void PresentWaitPrompt()
    {
        if (Console.IsInputRedirected)
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ResetColor();
        Console.ReadKey(true);
    }
}
