using HotPizza.Application.DTOs;
using HotPizza.Application.Exceptions;
using HotPizza.Application.Interfaces;
using HotPizza.Application.Mappers;

namespace HotPizza.Application.UseCases;

public interface ICreatePizzaUseCase
{
    Task<PizzaDto> ExecuteAsync(CreatePizzaRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Caso de uso: Registrar y validar una nueva pizza en el catálogo.
/// </summary>
public class CreatePizzaUseCase : ICreatePizzaUseCase
{
    private readonly IPizzaRepository _repository;
    private readonly IInputValidator<CreatePizzaRequest> _validator;

    public CreatePizzaUseCase(IPizzaRepository repository, IInputValidator<CreatePizzaRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<PizzaDto> ExecuteAsync(CreatePizzaRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validación de entrada (Input Validation)
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // 2. Verificar duplicados por nombre
        var existing = await _repository.GetByNameAsync(request.Nombre!.Trim(), cancellationToken);
        if (existing is not null)
        {
            throw new DuplicatePizzaException(request.Nombre!.Trim());
        }

        // 3. Crear entidad aplicando las reglas del dominio
        var entity = PizzaMapper.ToEntity(request);

        // 4. Persistir a través del repositorio
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // 5. Retornar DTO representativo
        return PizzaMapper.ToDto(entity);
    }
}
