using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HotPizza.Application.Interfaces;
using HotPizza.Domain.Entities;

namespace HotPizza.Infrastructure.Repositories;

/// <summary>
/// Adaptador de Gateway de Persistencia en archivo JSON (Interface Adapter).
/// Implementa IPizzaRepository para persistir las entidades en el archivo 'hotPizza.json'.
/// </summary>
public class JsonPizzaRepository : IPizzaRepository
{
    private readonly string _filePath;
    private readonly List<Pizza> _inMemoryCatalog = new();
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string StorageLocation => Path.GetFullPath(_filePath);

    public JsonPizzaRepository(string filePath = "hotPizza.json")
    {
        _filePath = string.IsNullOrWhiteSpace(filePath) ? "hotPizza.json" : filePath;
        InitializeAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_filePath))
            {
                var seedData = GetDefaultSeedData();
                _inMemoryCatalog.Clear();
                _inMemoryCatalog.AddRange(seedData);
                await SaveToFileAsync();
            }
            else
            {
                await LoadFromFileAsync();
            }
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<IReadOnlyList<Pizza>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            return _inMemoryCatalog.ToList().AsReadOnly();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<Pizza?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            return _inMemoryCatalog.FirstOrDefault(p =>
                p.Nombre.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task AddAsync(Pizza pizza, CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            _inMemoryCatalog.Add(pizza);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            await SaveToFileAsync();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            await LoadFromFileAsync();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<string> GetRawStorageContentAsync(CancellationToken cancellationToken = default)
    {
        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(_filePath)) return "[]";
            return await File.ReadAllTextAsync(_filePath, System.Text.Encoding.UTF8, cancellationToken);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task LoadFromFileAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(_filePath, System.Text.Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
            {
                _inMemoryCatalog.Clear();
                return;
            }

            var items = JsonSerializer.Deserialize<List<PizzaStorageModel>>(json, JsonOptions);
            _inMemoryCatalog.Clear();

            if (items is not null)
            {
                foreach (var item in items)
                {
                    _inMemoryCatalog.Add(new Pizza(
                        item.Nombre,
                        item.Descripcion,
                        item.Precio,
                        item.Tamaño,
                        item.Imagen
                    ));
                }
            }
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"El archivo de catálogo '{_filePath}' contiene un formato JSON corrupto o inválido: {ex.Message}", ex);
        }
        catch (IOException ex)
        {
            throw new IOException($"Error de entrada/salida al acceder al archivo '{_filePath}': {ex.Message}", ex);
        }
    }

    private async Task SaveToFileAsync()
    {
        try
        {
            var storageModels = _inMemoryCatalog.Select(p => new PizzaStorageModel
            {
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Tamaño = p.Tamaño,
                Imagen = p.Imagen
            }).ToList();

            var directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(storageModels, JsonOptions);
            await File.WriteAllTextAsync(_filePath, json, System.Text.Encoding.UTF8);
        }
        catch (IOException ex)
        {
            throw new IOException($"No se pudo escribir en el archivo '{_filePath}': {ex.Message}", ex);
        }
    }

    private static List<Pizza> GetDefaultSeedData() => new()
    {
        new Pizza(
            "Pizza Pepperoni Clásica",
            "Salsa de tomate casera, queso mozzarella fundido y abundante pepperoni crujiente.",
            149.50m,
            "Grande",
            "https://hotpizza.com/images/pepperoni-clasica.jpg"
        ),
        new Pizza(
            "Pizza Cuatro Quesos",
            "Exquisita combinación de mozzarella, gorgonzola, parmesano y gouda artesanal.",
            179.00m,
            "Familiar",
            "https://hotpizza.com/images/cuatro-quesos.jpg"
        ),
        new Pizza(
            "Pizza Hawaiana Especial",
            "Jamón de pierna seleccionado, trozos frescos de piña caramelizada y queso extra.",
            139.00m,
            "Mediana",
            "https://hotpizza.com/images/hawaiana-especial.jpg"
        ),
        new Pizza(
            "Pizza Carnes Supremas",
            "Pepperoni, salchicha italiana, jamón ahumado, tocino crujiente y carne de res.",
            199.99m,
            "Familiar",
            "https://hotpizza.com/images/carnes-supremas.jpg"
        ),
        new Pizza(
            "Pizza Vegetariana Gourmet",
            "Pimientos asados, champiñones frescos, cebolla morada, aceitunas negras y albahaca.",
            145.00m,
            "Mediana",
            "https://hotpizza.com/images/vegetariana-gourmet.jpg"
        ),
        new Pizza(
            "Pizza Mexicana Picante",
            "Chorizo artesanal, jalapeños en rodajas, frijoles refritos, cebolla y carne molida.",
            165.50m,
            "Grande",
            "https://hotpizza.com/images/mexicana-picante.jpg"
        )
    };

    /// <summary>
    /// Modelo de almacenamiento interno con los nombres de propiedad exactos para el JSON.
    /// </summary>
    private class PizzaStorageModel
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("tamaño")]
        public string Tamaño { get; set; } = string.Empty;

        [JsonPropertyName("imagen")]
        public string Imagen { get; set; } = string.Empty;
    }
}
