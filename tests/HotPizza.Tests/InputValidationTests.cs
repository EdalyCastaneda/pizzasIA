using HotPizza.Application.DTOs;
using HotPizza.Application.Validators;
using Xunit;

namespace HotPizza.Tests;

public class InputValidationTests
{
    private readonly CreatePizzaRequestValidator _validator = new();

    [Fact]
    public void Validate_ValidRequest_ShouldPassValidation()
    {
        var request = new CreatePizzaRequest(
            Nombre: "Pizza Hawaiana",
            Descripcion: "Jamón selecto y trozos de piña caramelizada con mozzarella",
            Precio: 140.00m,
            Tamaño: "Mediana",
            Imagen: "https://hotpizza.com/images/hawaiana.jpg"
        );

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_NullOrEmptyFields_ShouldFailWithMultipleErrors()
    {
        var request = new CreatePizzaRequest(
            Nombre: "",
            Descripcion: "",
            Precio: null,
            Tamaño: "",
            Imagen: ""
        );

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 5);
    }

    [Fact]
    public void Validate_InvalidPriceAndSize_ShouldContainSpecificErrors()
    {
        var request = new CreatePizzaRequest(
            Nombre: "Pizza Suprema",
            Descripcion: "Una gran variedad de carnes frías y verduras frescas",
            Precio: -10m,
            Tamaño: "ExtraGrande",
            Imagen: "https://hotpizza.com/images/suprema.jpg"
        );

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("precio", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(result.Errors, e => e.Contains("tamaño", StringComparison.OrdinalIgnoreCase));
    }
}
