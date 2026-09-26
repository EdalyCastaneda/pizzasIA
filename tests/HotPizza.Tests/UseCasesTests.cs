using HotPizza.Application.DTOs;
using HotPizza.Application.Exceptions;
using HotPizza.Application.UseCases;
using HotPizza.Application.Validators;
using HotPizza.Infrastructure.Repositories;
using Xunit;

namespace HotPizza.Tests;

public class UseCasesTests : IDisposable
{
    private readonly string _testJsonPath;
    private readonly JsonPizzaRepository _repository;
    private readonly CreatePizzaRequestValidator _validator;

    public UseCasesTests()
    {
        _testJsonPath = Path.Combine(Path.GetTempPath(), $"hotpizza_test_{Guid.NewGuid():N}.json");
        _repository = new JsonPizzaRepository(_testJsonPath);
        _validator = new CreatePizzaRequestValidator();
    }

    public void Dispose()
    {
        if (File.Exists(_testJsonPath))
        {
            try { File.Delete(_testJsonPath); } catch { }
        }
    }

    [Fact]
    public async Task GetPizzaCatalogUseCase_ShouldReturnSeededPizzas()
    {
        var useCase = new GetPizzaCatalogUseCase(_repository);

        var catalog = await useCase.ExecuteAsync();

        Assert.NotEmpty(catalog);
        Assert.Contains(catalog, p => p.Nombre == "Pizza Pepperoni Clásica");
    }

    [Fact]
    public async Task CreatePizzaUseCase_WithValidData_ShouldAddAndPersist()
    {
        var useCase = new CreatePizzaUseCase(_repository, _validator);
        var request = new CreatePizzaRequest(
            Nombre: "Pizza BBQ Especial",
            Descripcion: "Pollo a la parrilla bañado en salsa barbacoa y cebolla morada",
            Precio: 169.00m,
            Tamaño: "Grande",
            Imagen: "https://hotpizza.com/images/bbq.jpg"
        );

        var created = await useCase.ExecuteAsync(request);

        Assert.NotNull(created);
        Assert.Equal("Pizza BBQ Especial", created.Nombre);

        // Verify retrieval
        var existing = await _repository.GetByNameAsync("Pizza BBQ Especial");
        Assert.NotNull(existing);
    }

    [Fact]
    public async Task CreatePizzaUseCase_WithDuplicateName_ShouldThrowDuplicatePizzaException()
    {
        var useCase = new CreatePizzaUseCase(_repository, _validator);
        var request = new CreatePizzaRequest(
            Nombre: "Pizza Pepperoni Clásica", // Already seeded
            Descripcion: "Descripción repetida para probar duplicados en catálogo",
            Precio: 150.00m,
            Tamaño: "Grande",
            Imagen: "https://hotpizza.com/images/pepperoni.jpg"
        );

        await Assert.ThrowsAsync<DuplicatePizzaException>(() => useCase.ExecuteAsync(request));
    }

    [Fact]
    public async Task CreatePizzaUseCase_WithInvalidInput_ShouldThrowValidationException()
    {
        var useCase = new CreatePizzaUseCase(_repository, _validator);
        var request = new CreatePizzaRequest(
            Nombre: "P", // Demasiado corto
            Descripcion: "corta",
            Precio: -10,
            Tamaño: "Invalido",
            Imagen: ""
        );

        await Assert.ThrowsAsync<ValidationException>(() => useCase.ExecuteAsync(request));
    }
}
