using System.Text;
using Microsoft.Extensions.DependencyInjection;
using HotPizza.Application.DTOs;
using HotPizza.Application.Interfaces;
using HotPizza.Application.UseCases;
using HotPizza.Application.Validators;
using HotPizza.ErrorHandling;
using HotPizza.Infrastructure.Repositories;
using HotPizza.Presentation.Controllers;
using HotPizza.Presentation.Presenters;
using HotPizza.UI;

namespace HotPizza;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Configuración de Consola
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "HotPizza - Clean Architecture (Uncle Bob)";

        // 1. Inicialización del Manejador Global de Errores
        GlobalErrorHandler.Initialize();

        // 2. Configuración de Inyección de Dependencias (DI Container)
        var services = new ServiceCollection();
        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        // 3. Ejecución de la aplicación a través de su punto de entrada seguro
        await GlobalErrorHandler.ExecuteSafelyAsync(async () =>
        {
            var runner = serviceProvider.GetRequiredService<ConsoleAppRunner>();
            await runner.RunAsync();
        }, "Ciclo principal de ejecución");
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Capa de Infraestructura (Interface Adapters / Persistence Gateways)
        services.AddSingleton<IPizzaRepository>(sp => new JsonPizzaRepository("hotPizza.json"));

        // Capa de Aplicación: Validadores (Input Validation)
        services.AddTransient<IInputValidator<CreatePizzaRequest>, CreatePizzaRequestValidator>();

        // Capa de Aplicación: Casos de Uso (Application Business Rules)
        services.AddTransient<IGetPizzaCatalogUseCase, GetPizzaCatalogUseCase>();
        services.AddTransient<ICreatePizzaUseCase, CreatePizzaUseCase>();
        services.AddTransient<ISearchPizzaUseCase, SearchPizzaUseCase>();
        services.AddTransient<ISavePizzaCatalogUseCase, SavePizzaCatalogUseCase>();
        services.AddTransient<IReloadPizzaCatalogUseCase, ReloadPizzaCatalogUseCase>();
        services.AddTransient<IGetStorageInfoUseCase, GetStorageInfoUseCase>();

        // Capa de Presentación (Interface Adapters)
        services.AddSingleton<IPizzaPresenter, ConsolePizzaPresenter>();
        services.AddTransient<PizzaController>();

        // Capa de Frameworks & Drivers (Console UI Runner)
        services.AddTransient<ConsoleAppRunner>();
    }
}
