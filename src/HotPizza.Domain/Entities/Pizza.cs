using HotPizza.Domain.Enums;
using HotPizza.Domain.Exceptions;

namespace HotPizza.Domain.Entities;

/// <summary>
/// Entidad central de negocio: Pizza.
/// Representa las reglas empresariales (Enterprise Business Rules) en la capa más interna de Clean Architecture.
/// </summary>
public class Pizza
{
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public decimal Precio { get; private set; }
    public string Tamaño { get; private set; } = string.Empty;
    public string Imagen { get; private set; } = string.Empty;

    // Constructor sin parámetros para deserialización
    public Pizza() { }

    /// <summary>
    /// Constructor principal que aplica las reglas invariantes de negocio de la entidad.
    /// </summary>
    public Pizza(string nombre, string descripcion, decimal precio, string tamaño, string imagen)
    {
        SetNombre(nombre);
        SetDescripcion(descripcion);
        SetPrecio(precio);
        SetTamaño(tamaño);
        SetImagen(imagen);
    }

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new InvalidPizzaDataException(nameof(Nombre), "El nombre de la pizza no puede estar vacío.");

        if (nombre.Trim().Length < 3 || nombre.Trim().Length > 80)
            throw new InvalidPizzaDataException(nameof(Nombre), "El nombre debe tener entre 3 y 80 caracteres.");

        Nombre = nombre.Trim();
    }

    public void SetDescripcion(string descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
            throw new InvalidPizzaDataException(nameof(Descripcion), "La descripción no puede estar vacía.");

        if (descripcion.Trim().Length < 10 || descripcion.Trim().Length > 300)
            throw new InvalidPizzaDataException(nameof(Descripcion), "La descripción debe tener entre 10 y 300 caracteres.");

        Descripcion = descripcion.Trim();
    }

    public void SetPrecio(decimal precio)
    {
        if (precio <= 0)
            throw new InvalidPizzaDataException(nameof(Precio), "El precio debe ser un valor positivo mayor a cero.");

        if (precio > 10000)
            throw new InvalidPizzaDataException(nameof(Precio), "El precio no puede exceder los $10,000.00 MXN.");

        Precio = decimal.Round(precio, 2);
    }

    public void SetTamaño(string tamaño)
    {
        if (string.IsNullOrWhiteSpace(tamaño))
            throw new InvalidPizzaDataException(nameof(Tamaño), "El tamaño es requerido.");

        if (!PizzaSizeExtensions.TryParseFriendly(tamaño, out var parsedSize))
            throw new InvalidPizzaDataException(nameof(Tamaño), $"El tamaño '{tamaño}' no es válido. Opciones permitidas: Personal, Mediana, Grande, Familiar.");

        Tamaño = parsedSize.ToFriendlyString();
    }

    public void SetImagen(string imagen)
    {
        if (string.IsNullOrWhiteSpace(imagen))
            throw new InvalidPizzaDataException(nameof(Imagen), "La URL o ruta de la imagen no puede estar vacía.");

        Imagen = imagen.Trim();
    }
}
