using HotPizza.Application.DTOs;

namespace HotPizza.Presentation.Presenters;

/// <summary>
/// Interfaz del Presenter (Interface Adapter en Clean Architecture).
/// Separa la lógica de formateo y renderizado de la lógica del controlador.
/// </summary>
public interface IPizzaPresenter
{
    void PresentWelcome(string storageLocation);
    void PresentMenu();
    void PresentCatalog(IReadOnlyList<PizzaDto> pizzas);
    void PresentPizzaDetail(PizzaDto pizza);
    void PresentSuccess(string message);
    void PresentWarning(string message);
    void PresentError(string message, IEnumerable<string>? details = null);
    void PresentJsonContent(string json, string filePath);
    void PresentGoodbye();
    void PresentDivider();
    void PresentWaitPrompt();
}
