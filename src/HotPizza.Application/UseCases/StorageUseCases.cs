using HotPizza.Application.Interfaces;

namespace HotPizza.Application.UseCases;

public interface ISavePizzaCatalogUseCase
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class SavePizzaCatalogUseCase : ISavePizzaCatalogUseCase
{
    private readonly IPizzaRepository _repository;

    public SavePizzaCatalogUseCase(IPizzaRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _repository.SaveChangesAsync(cancellationToken);
    }
}

public interface IReloadPizzaCatalogUseCase
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

public class ReloadPizzaCatalogUseCase : IReloadPizzaCatalogUseCase
{
    private readonly IPizzaRepository _repository;

    public ReloadPizzaCatalogUseCase(IPizzaRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _repository.ReloadAsync(cancellationToken);
    }
}

public interface IGetStorageInfoUseCase
{
    string GetStorageLocation();
    Task<string> GetRawStorageContentAsync(CancellationToken cancellationToken = default);
}

public class GetStorageInfoUseCase : IGetStorageInfoUseCase
{
    private readonly IPizzaRepository _repository;

    public GetStorageInfoUseCase(IPizzaRepository repository)
    {
        _repository = repository;
    }

    public string GetStorageLocation() => _repository.StorageLocation;

    public Task<string> GetRawStorageContentAsync(CancellationToken cancellationToken = default) =>
        _repository.GetRawStorageContentAsync(cancellationToken);
}
