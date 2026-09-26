using HotPizza.Application.Common;
using HotPizza.Application.DTOs;
using HotPizza.Application.Interfaces;
using HotPizza.Domain.Enums;

namespace HotPizza.Application.Validators;

/// <summary>
/// Validador de entrada para solicitudes de creación de pizza.
/// Asegura que los datos proporcionados por el usuario cumplan las especificaciones antes de llegar a las entidades.
/// </summary>
public class CreatePizzaRequestValidator : IInputValidator<CreatePizzaRequest>
{
    public ValidationResult Validate(CreatePizzaRequest input)
    {
        var result = new ValidationResult();

        if (input is null)
        {
            result.AddError("La solicitud no puede ser nula.");
            return result;
        }

        // Validación de Nombre
        if (string.IsNullOrWhiteSpace(input.Nombre))
        {
            result.AddError("El nombre de la pizza es obligatorio y no puede contener solo espacios.");
        }
        else
        {
            var nombre = input.Nombre.Trim();
            if (nombre.Length < 3)
            {
                result.AddError("El nombre de la pizza debe contener al menos 3 caracteres.");
            }
            else if (nombre.Length > 80)
            {
                result.AddError("El nombre de la pizza no puede exceder los 80 caracteres.");
            }
        }

        // Validación de Descripción
        if (string.IsNullOrWhiteSpace(input.Descripcion))
        {
            result.AddError("La descripción es obligatoria.");
        }
        else
        {
            var desc = input.Descripcion.Trim();
            if (desc.Length < 10)
            {
                result.AddError("La descripción debe tener al menos 10 caracteres para describir adecuadamente la pizza.");
            }
            else if (desc.Length > 300)
            {
                result.AddError("La descripción no debe exceder los 300 caracteres.");
            }
        }

        // Validación de Precio
        if (!input.Precio.HasValue)
        {
            result.AddError("El precio de la pizza es obligatorio.");
        }
        else if (input.Precio.Value <= 0)
        {
            result.AddError("El precio debe ser un valor positivo mayor a 0.");
        }
        else if (input.Precio.Value > 10000)
        {
            result.AddError("El precio no puede exceder los $10,000.00 MXN.");
        }

        // Validación de Tamaño
        if (string.IsNullOrWhiteSpace(input.Tamaño))
        {
            result.AddError("El tamaño de la pizza es obligatorio (Personal, Mediana, Grande, Familiar).");
        }
        else if (!PizzaSizeExtensions.TryParseFriendly(input.Tamaño, out _))
        {
            result.AddError($"El tamaño '{input.Tamaño}' no es válido. Los tamaños aceptados son: Personal, Mediana, Grande o Familiar.");
        }

        // Validación de Imagen
        if (string.IsNullOrWhiteSpace(input.Imagen))
        {
            result.AddError("La URL o ruta de la imagen es obligatoria.");
        }
        else
        {
            var img = input.Imagen.Trim();
            bool isUri = Uri.TryCreate(img, UriKind.Absolute, out var uriResult) 
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            bool isLocalPath = img.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                               || img.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                               || img.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                               || img.EndsWith(".webp", StringComparison.OrdinalIgnoreCase);

            if (!isUri && !isLocalPath)
            {
                result.AddError("La imagen debe ser una URL válida (http/https) o un archivo de imagen compatible (.jpg, .jpeg, .png, .webp).");
            }
        }

        return result;
    }
}
