using HotPizza.ErrorHandling;
using HotPizza.Presentation.Controllers;
using HotPizza.Presentation.Presenters;

namespace HotPizza.UI;

/// <summary>
/// Orquestador del flujo de la interfaz de consola en la capa de Frameworks & Drivers.
/// </summary>
public class ConsoleAppRunner
{
    private readonly PizzaController _controller;
    private readonly IPizzaPresenter _presenter;

    public ConsoleAppRunner(PizzaController controller, IPizzaPresenter presenter)
    {
        _controller = controller;
        _presenter = presenter;
    }

    public async Task RunAsync()
    {
        _presenter.PresentWelcome(_controller.ObtenerUbicacionAlmacenamiento());

        bool salir = false;
        while (!salir)
        {
            _presenter.PresentMenu();
            string? opcion = Console.ReadLine()?.Trim();

            await GlobalErrorHandler.ExecuteSafelyAsync(async () =>
            {
                switch (opcion)
                {
                    case "1":
                        await _controller.VerCatalogoAsync();
                        break;
                    case "2":
                        var request = ConsoleInputReader.PromptForNewPizza();
                        await _controller.RegistrarPizzaAsync(request);
                        break;
                    case "3":
                        var query = ConsoleInputReader.ReadSearchQuery();
                        await _controller.BuscarPizzasAsync(query);
                        break;
                    case "4":
                        await _controller.GuardarCatalogoAsync();
                        break;
                    case "5":
                        await _controller.RecargarCatalogoAsync();
                        break;
                    case "6":
                        await _controller.VerContenidoJsonAsync();
                        break;
                    case "7":
                        salir = true;
                        _presenter.PresentGoodbye();
                        break;
                    default:
                        _presenter.PresentWarning("Opción no válida. Por favor ingrese un número del 1 al 7.");
                        break;
                }
            }, $"Procesamiento de opción de menú: {opcion}");

            if (!salir)
            {
                _presenter.PresentWaitPrompt();
            }
        }
    }
}
