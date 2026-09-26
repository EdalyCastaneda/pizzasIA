namespace HotPizza.ErrorHandling;

/// <summary>
/// Manejador global de errores (Global Error Handler) en la capa de Frameworks & Drivers.
/// Captura y procesa excepciones no controladas a nivel de aplicación para garantizar estabilidad y claridad.
/// </summary>
public static class GlobalErrorHandler
{
    public static void Initialize()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            HandleFatalException(ex);
        };

        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            HandleException(args.Exception, "Error en tarea en segundo plano");
            args.SetObserved();
        };
    }

    public static async Task ExecuteSafelyAsync(Func<Task> action, string contextDescription)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            HandleException(ex, contextDescription);
        }
    }

    public static void HandleException(Exception? ex, string context = "Operación general")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║               MANEJADOR GLOBAL DE ERRORES                     ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
        Console.WriteLine($"[Contexto] {context}");
        Console.WriteLine($"[Tipo de Excepción] {ex?.GetType().FullName ?? "Desconocido"}");
        Console.WriteLine($"[Mensaje] {ex?.Message ?? "No se proporcionó detalle del error."}");

        if (ex?.InnerException != null)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[Causa interna] {ex.InnerException.Message}");
        }

        Console.ResetColor();
    }

    private static void HandleFatalException(Exception? ex)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\n[ERROR FATAL NO CONTROLADO] La aplicación experimentó una falla crítica:");
        Console.WriteLine(ex?.ToString());
        Console.ResetColor();
    }
}
