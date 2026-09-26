# 🍕 HotPizza - Clean Architecture (Arquitectura Limpia)

> Proyecto desarrollado en **C# (.NET 10)** para el curso **cursoIA** implementando la estructura de **Arquitectura Limpia (Clean Architecture)** basada en el modelo clásico de Robert C. Martin (Uncle Bob).

---

## 🏛️ Referencia Arquitectónica: Clean Architecture (Uncle Bob)

El diseño del proyecto sigue estrictamente los cuatro círculos concéntricos de la Arquitectura Limpia ([Clean Architecture de Robert C. Martin](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)):

```
                        ┌────────────────────────────────────────┐
                        │     Frameworks & Drivers (Exterior)    │
                        │  • HotPizza (Consola, Program, DI)     │
                        │  • UI, Sistema de Archivos             │
                        │  ┌──────────────────────────────────┐  │
                        │  │       Interface Adapters         │  │
                        │  │  • HotPizza.Infrastructure       │  │
                        │  │    (JsonPizzaRepository Gateway) │  │
                        │  │  • HotPizza.Presentation         │  │
                        │  │    (PizzaController, Presenter)  │  │
                        │  │  ┌────────────────────────────┐  │  │
                        │  │  │        Use Cases           │  │  │
                        │  │  │  • HotPizza.Application    │  │  │
                        │  │  │    (Casos de uso, DTOs,    │  │  │
                        │  │  │     Validadores, Puertos)  │  │  │
                        │  │  │  ┌──────────────────────┐  │  │  │
                        │  │  │  │       Entities       │  │  │  │
                        │  │  │  │  • HotPizza.Domain   │  │  │  │
                        │  │  │  │    (Pizza, Enums,    │  │  │  │
                        │  │  │  │     Reglas de Neg.)  │  │  │  │
                        │  │  │  └──────────────────────┘  │  │  │
                        │  │  └────────────────────────────┘  │  │
                        │  └──────────────────────────────────┘  │
                        └────────────────────────────────────────┘
```

### 1. 🟡 Capa de Entidades (`HotPizza.Domain`) - *Enterprise Business Rules*
- **`Pizza`**: Entidad central que encapsula el estado y las reglas de negocio invariantes (precios mayores a cero, nombres no vacíos, tamaños permitidos y URLs/rutas válidas).
- **`PizzaSize` & `PizzaSizeExtensions`**: Enum con tamaños predefinidos (`Personal`, `Mediana`, `Grande`, `Familiar`) y mapeo amigable.
- **`DomainException` e `InvalidPizzaDataException`**: Excepciones específicas de dominio para asegurar la integridad de las entidades.
- **Regla de dependencia**: Esta capa no depende de ningún paquete externo ni de capas superiores.

### 2. 🔴 Capa de Casos de Uso (`HotPizza.Application`) - *Application Business Rules*
- **Casos de Uso**:
  - `GetPizzaCatalogUseCase`: Recuperación del catálogo completo.
  - `CreatePizzaUseCase`: Orquesta la validación de entrada, verificación de duplicados, creación de la entidad y persistencia.
  - `SearchPizzaUseCase`: Búsqueda de pizzas por coincidencia en nombre, ingredientes o tamaño.
  - `SavePizzaCatalogUseCase` & `ReloadPizzaCatalogUseCase`: Persistencia y recarga del catálogo.
  - `GetStorageInfoUseCase`: Consulta de ruta y contenido crudo del almacenamiento.
- **Puertos / Gateways (DIP)**: `IPizzaRepository` define el contrato de persistencia sin conocer los detalles de implementación (archivo JSON, base de datos, etc.).
- **Validación de Entrada**:
  - `IInputValidator<T>`: Contrato genérico de validación.
  - `CreatePizzaRequestValidator`: Validador que evalúa los campos requeridos, longitudes mínimas y máximas, formato de precio, validez de tamaño y formato de imagen.
  - `ValidationResult`: Contenedor de estado y mensajes de error.
- **Excepciones de Aplicación**: `ValidationException`, `DuplicatePizzaException`, `PizzaNotFoundException`.

### 3. 🟢 Capa de Adaptadores de Interfaz (`HotPizza.Infrastructure` & `HotPizza.Presentation`) - *Interface Adapters*
- **`HotPizza.Infrastructure` (Gateways / Persistencia)**:
  - `JsonPizzaRepository`: Implementación de `IPizzaRepository` que lee y escribe sobre el archivo `hotPizza.json` con soporte concurrente seguro (`SemaphoreSlim`) y manejo de errores de E/S y JSON corrupto.
- **`HotPizza.Presentation` (Controladores y Presenters)**:
  - `PizzaController`: Recibe comandos del usuario, interactúa con los casos de uso y entrega los resultados al Presenter.
  - `IPizzaPresenter` & `ConsolePizzaPresenter`: Presenter desacoplado que formatea tablas con colores, resúmenes, mensajes de éxito y errores visuales.

### 4. 🔵 Capa de Frameworks & Drivers (`HotPizza`) - *UI & Dispositivos*
- **`Program.cs`**: Punto de entrada con contenedor de Inyección de Dependencias (`Microsoft.Extensions.DependencyInjection`).
- **`GlobalErrorHandler`**: Manejador global de excepciones para capturar errores no controlados, eventos `AppDomain.UnhandledException` y tareas no observadas, evitando el cierre abrupto de la aplicación.
- **`ConsoleInputReader`**: Lector de consola que solicita los datos paso a paso con validación interactiva y sugerencias automáticas.
- **`ConsoleAppRunner`**: Bucle principal de interacción de consola.

---

## 🍕 Catálogo de Pizzas (`hotPizza.json`)

El catálogo se almacena en `hotPizza.json` respetando el formato requerido:
- `nombre`: Nombre comercial de la pizza.
- `descripcion`: Ingredientes y descripción culinaria.
- `precio`: Precio en moneda nacional (MXN).
- `tamaño`: Tamaño (`Personal`, `Mediana`, `Grande`, `Familiar`).
- `imagen`: Enlace HTTP/HTTPS o ruta local de imagen.

### Ejemplo en `hotPizza.json`:
```json
[
  {
    "nombre": "Pizza Pepperoni Clásica",
    "descripcion": "Salsa de tomate casera, queso mozzarella fundido y abundante pepperoni crujiente.",
    "precio": 149.50,
    "tamaño": "Grande",
    "imagen": "https://hotpizza.com/images/pepperoni-clasica.jpg"
  },
  {
    "nombre": "Pizza Cuatro Quesos",
    "descripcion": "Exquisita combinación de mozzarella, gorgonzola, parmesano y gouda artesanal.",
    "precio": 179.00,
    "tamaño": "Familiar",
    "imagen": "https://hotpizza.com/images/cuatro-quesos.jpg"
  }
]
```

---

## 🛡️ Manejo de Errores y Validación de Entrada

1. **Validación Temprana**: Antes de procesar cualquier pizza, `CreatePizzaRequestValidator` valida exhaustivamente los tipos, rangos y obligatoriedad.
2. **Validación en Dominio**: La entidad `Pizza` garantiza que ninguna regla de negocio pueda ser violada (invariantes de dominio).
3. **Manejador Global de Errores**:
   - `GlobalErrorHandler.ExecuteSafelyAsync`: Envuelve cada opción del menú para capturar excepciones inesperadas.
   - Captura eventos no observados en `AppDomain` y `TaskScheduler`.
   - Muestra mensajes limpios y explicativos al usuario sin romper el ciclo de vida de la aplicación.

---

## 🧪 Pruebas Unitarias (`HotPizza.Tests`)

Se incluye una suite de pruebas con **xUnit** con 16 pruebas que verifican:
- Reglas de dominio de `Pizza` (precios negativos, nombres cortos, tamaños inválidos).
- Validador de entrada `CreatePizzaRequestValidator`.
- Casos de uso (`GetPizzaCatalogUseCase`, `CreatePizzaUseCase`, detección de duplicados).

Para ejecutar las pruebas:
```bash
dotnet test
```

---

## 🚀 Compilación y Ejecución

### Compilar la solución completa:
```bash
dotnet build
```

### Ejecutar la aplicación de consola:
```bash
dotnet run --project src/HotPizza/HotPizza.csproj
```
