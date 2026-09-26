using HotPizza.Domain.Entities;
using HotPizza.Domain.Exceptions;
using Xunit;

namespace HotPizza.Tests;

public class PizzaEntityTests
{
    [Fact]
    public void Pizza_Creation_WithValidData_ShouldSucceed()
    {
        // Act
        var pizza = new Pizza(
            "Pizza Pepperoni",
            "Deliciosa pizza artesanal con pepperoni crujiente y salsa de tomate",
            150.00m,
            "Grande",
            "https://hotpizza.com/images/pepperoni.jpg"
        );

        // Assert
        Assert.Equal("Pizza Pepperoni", pizza.Nombre);
        Assert.Equal(150.00m, pizza.Precio);
        Assert.Equal("Grande", pizza.Tamaño);
    }

    [Theory]
    [InlineData("", "Descripción válida con más de 10 caracteres", 100, "Grande", "https://img.com/p.jpg")]
    [InlineData("AB", "Descripción válida con más de 10 caracteres", 100, "Grande", "https://img.com/p.jpg")]
    public void Pizza_Creation_WithInvalidName_ShouldThrowInvalidPizzaDataException(
        string nombre, string desc, decimal precio, string tamaño, string imagen)
    {
        Assert.Throws<InvalidPizzaDataException>(() => new Pizza(nombre, desc, precio, tamaño, imagen));
    }

    [Theory]
    [InlineData("Pizza Test", "Corta", 100, "Grande", "https://img.com/p.jpg")]
    public void Pizza_Creation_WithShortDescription_ShouldThrowInvalidPizzaDataException(
        string nombre, string desc, decimal precio, string tamaño, string imagen)
    {
        Assert.Throws<InvalidPizzaDataException>(() => new Pizza(nombre, desc, precio, tamaño, imagen));
    }

    [Theory]
    [InlineData("Pizza Test", "Descripción válida con más de 10 caracteres", 0, "Grande", "https://img.com/p.jpg")]
    [InlineData("Pizza Test", "Descripción válida con más de 10 caracteres", -50, "Grande", "https://img.com/p.jpg")]
    public void Pizza_Creation_WithZeroOrNegativePrice_ShouldThrowInvalidPizzaDataException(
        string nombre, string desc, decimal precio, string tamaño, string imagen)
    {
        Assert.Throws<InvalidPizzaDataException>(() => new Pizza(nombre, desc, precio, tamaño, imagen));
    }

    [Theory]
    [InlineData("Gigante")]
    [InlineData("Mini")]
    [InlineData("Invalido")]
    public void Pizza_Creation_WithInvalidSize_ShouldThrowInvalidPizzaDataException(string invalidSize)
    {
        Assert.Throws<InvalidPizzaDataException>(() => new Pizza(
            "Pizza Test",
            "Descripción válida con más de 10 caracteres",
            120.00m,
            invalidSize,
            "https://img.com/p.jpg"
        ));
    }
}
