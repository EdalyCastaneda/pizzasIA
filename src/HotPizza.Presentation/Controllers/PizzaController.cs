using HotPizza.Application.DTOs;
using HotPizza.Application.Exceptions;
using HotPizza.Application.UseCases;
using HotPizza.Domain.Exceptions;
using HotPizza.Presentation.Presenters;

namespace HotPizza.Presentation.Controllers;

/// <summary>
/// Controlador de la Pizza (Interface Adapter en Clean Architecture).
/// Recibe peticiones de la interfaz de usuario, orquesta los casos de uso
/// y envía las respuestas al Presenter.
/// </summary>
public class PizzaController
{
    private readonly IGetPizzaCatalogUseCase _getCatalogUseCase;
    private readonly ICreatePizzaUseCase _createPizzaUseCase;
    private readonly ISearchPizzaUseCase _searchPizzaUseCase;
    private readonly ISavePizzaCatalogUseCase _saveCatalogUseCase;
    private readonly IReloadPizzaCatalogUseCase _reloadCatalogUseCase;
    private readonly IGetStorageInfoUseCase _storageInfoUseCase;
    private readonly IPizzaPresenter _presenter;

    public PizzaController(
        IGetPizzaCatalogUseCase getCatalogUseCase,
        ICreatePizzaUseCase createPizzaUseCase,
        ISearchPizzaUseCase searchPizzaUseCase,
        ISavePizzaCatalogUseCase saveCatalogUseCase,
        IReloadPizzaCatalogUseCase reloadCatalogUseCase,
        IGetStorageInfoUseCase storageInfoUseCase,
        IPizzaPresenter presenter)
    {
        _getCatalogUseCase = getCatalogUseCase;
        _createPizzaUseCase = createPizzaUseCase;
        _searchPizzaUseCase = searchPizzaUseCase;
        _saveCatalogUseCase = saveCatalogUseCase;
        _reloadCatalogUseCase = reloadCatalogUseCase;
        _storageInfoUseCase = storageInfoUseCase;
        _presenter = presenter;
    }

    public string ObtenerUbicacionAlmacenamiento() => _storageInfoUseCase.GetStorageLocation();

    public async Task VerCatalogoAsync(CancellationToken cancellationToken = default)
    {
        var pizzas = await _getCatalogUseCase.ExecuteAsync(cancellationToken);
        _presenter.PresentCatalog(pizzas);
    }

    public async Task<bool> RegistrarPizzaAsync(CreatePizzaRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var created = await _createPizzaUseCase.ExecuteAsync(request, cancellationToken);
            _presenter.PresentSuccess($"Pizza '{created.Nombre}' registrada exitosamente y sincronizada en 'hotPizza.json'.");
            _presenter.PresentPizzaDetail(created);
            return true;
        }
        catch (ValidationException valEx)
        {
            _presenter.PresentError("Error de validación de entrada:", valEx.Errors);
            return false;
        }
        catch (DuplicatePizzaException dupEx)
        {
            _presenter.PresentError(dupEx.Message);
            return false;
        }
        catch (DomainException domEx)
        {
            _presenter.PresentError($"Violación de regla de dominio: {domEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            _presenter.PresentError($"Error inesperado al registrar pizza: {ex.Message}");
            return false;
        }
    }

    public async Task BuscarPizzasAsync(string query, CancellationToken cancellationToken = default)
    {
        var resultados = await _searchPizzaUseCase.ExecuteAsync(query, cancellationToken);
        if (resultados.Count == 0)
        {
            _presenter.PresentWarning($"No se encontraron pizzas que coincidan con el término '{query}'.");
            return;
        }

        _presenter.PresentCatalog(resultados);
    }

    public async Task GuardarCatalogoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _saveCatalogUseCase.ExecuteAsync(cancellationToken);
            _presenter.PresentSuccess($"Catálogo guardado satisfactoriamente en: {_storageInfoUseCase.GetStorageLocation()}");
        }
        catch (Exception ex)
        {
            _presenter.PresentError($"Error al guardar el archivo: {ex.Message}");
        }
    }

    public async Task RecargarCatalogoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _reloadCatalogUseCase.ExecuteAsync(cancellationToken);
            _presenter.PresentSuccess("Catálogo recargado correctamente desde 'hotPizza.json'.");
        }
        catch (Exception ex)
        {
            _presenter.PresentError($"Error al recargar el archivo: {ex.Message}");
        }
    }

    public async Task VerContenidoJsonAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var raw = await _storageInfoUseCase.GetRawStorageContentAsync(cancellationToken);
            _presenter.PresentJsonContent(raw, _storageInfoUseCase.GetStorageLocation());
        }
        catch (Exception ex)
        {
            _presenter.PresentError($"No se pudo leer el archivo JSON: {ex.Message}");
        }
    }
}
