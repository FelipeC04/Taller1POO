# Taller1POO — Sistema de Notas

Sistema educativo de consola desarrollado en **C# (.NET)** que permite gestionar estudiantes, materias y notas mediante operaciones CRUD, con persistencia en archivos CSV.

---

## Estructura del Proyecto

```
SistemaNotas/
├── Models/
│   ├── IIdentifiable.cs
│   ├── Persona.cs
│   ├── Direccion.cs
│   ├── Estudiante.cs
│   ├── Profesor.cs
│   ├── Materia.cs
│   └── Nota.cs
├── Repositories/
│   └── CsvRepository.cs
└── Menu.cs
```

---

## Models

### `IIdentifiable` — Interfaz

Define un contrato común para todas las entidades que necesitan ser identificadas por un `Id`.

```csharp
public interface IIdentifiable
{
    int Id { get; set; }
}
```

---

### `Persona` — Clase abstracta (Herencia)

Clase base abstracta que implementa `IIdentifiable`. Contiene las propiedades compartidas por `Estudiante` y `Profesor`.

```csharp
public abstract class Persona : IIdentifiable
{
    public int Id { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
}
```

---

### `Direccion` — Clase de valor (Composición)

Clase utilizada exclusivamente como parte de un modelo mayor. No tiene sentido por sí sola en el dominio y no implementa `IIdentifiable`.

```csharp
public class Direccion
{
    public string Calle { get; set; }
    public string Ciudad { get; set; }
}
```

---

### `Estudiante` — Herencia + Composición

Hereda de `Persona`. Incluye una propiedad `Matricula` y una relación de **composición** con `Direccion` (la dirección comparte el ciclo de vida del estudiante).

```csharp
public class Estudiante : Persona
{
    public string Matricula { get; set; }
    public Direccion DireccionResidencia { get; set; } = new Direccion();
}
```

---

### `Profesor` — Herencia

Hereda de `Persona`. Añade la propiedad `Especialidad`.

```csharp
public class Profesor : Persona
{
    public string Especialidad { get; set; }
}
```

---

### `Materia` — Agregación

Implementa `IIdentifiable`. Contiene una relación de **agregación** con `Profesor` a través de `ProfesorTitularId`: el profesor existe independientemente de la materia.

```csharp
public class Materia : IIdentifiable
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Creditos { get; set; }
    public int ProfesorTitularId { get; set; }
}
```

---

### `Nota` — Asociación

Clase puente de **asociación** entre `Estudiante` y `Materia`. Implementa `IIdentifiable`.

```csharp
public class Nota : IIdentifiable
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int MateriaId { get; set; }
    public decimal ValorNota { get; set; }
    public string Comentarios { get; set; }
}
```

---

## Repositories

### `CsvRepository<T>` — Repositorio genérico

Repositorio genérico que realiza operaciones **CRUD** sobre cualquier entidad que implemente `IIdentifiable`, utilizando archivos CSV como almacenamiento (con la librería **CsvHelper**).

```csharp
public class CsvRepository<T> where T : IIdentifiable
```

#### Constructor

Recibe el nombre de la entidad, crea la carpeta `Data/` si no existe y genera el archivo CSV correspondiente.

```csharp
public CsvRepository(string entityName)
```

#### Métodos

| Método | Descripción |
|---|---|
| `List<T> GetAll()` | Obtiene todas las entidades almacenadas en el CSV. |
| `void SaveAll(List<T> records)` | Sobrescribe el CSV con la lista de entidades proporcionada. |
| `void Create(T entity)` | Agrega una nueva entidad. Si el `Id` es 0, se asigna uno incremental automáticamente. |
| `T? Read(int id)` | Busca y retorna una entidad por su `Id`, o `null` si no existe. |
| `void Update(T entity)` | Actualiza una entidad existente buscándola por su `Id`. |
| `void Delete(int id)` | Elimina una entidad del CSV por su `Id`. |

---

## Menu (Punto de entrada)

`Menu.cs` contiene la clase `Program` con el método `Main`. Presenta un menú interactivo en consola con tres sub-menús:

1. **Gestión de Estudiantes** — Crear, listar, modificar y eliminar estudiantes.
2. **Gestión de Materias** — Crear, listar, modificar y eliminar materias.
3. **Gestión de Notas** — Asociar estudiantes a materias con una nota, listar, modificar y eliminar.

Incluye métodos de validación de entrada: `LeerTextoNoNumerico()`, `LeerTextoNoVacio()` y `LeerEnteroPositivo()`.

---

## Conceptos POO aplicados

| Concepto | Dónde se aplica |
|---|---|
| **Herencia** | `Persona` → `Estudiante`, `Profesor` |
| **Composición** | `Estudiante` contiene `Direccion` (ciclo de vida compartido) |
| **Agregación** | `Materia` referencia a `Profesor` por Id (ciclo de vida independiente) |
| **Asociación** | `Nota` relaciona `Estudiante` con `Materia` |
| **Interfaz** | `IIdentifiable` define el contrato de identificación |
| **Genéricos** | `CsvRepository<T>` opera sobre cualquier `IIdentifiable` |
| **Abstracción** | `Persona` es clase abstracta, no se instancia directamente |

---

## Cómo ejecutar

```bash
dotnet run --project SistemaNotas/SistemaNotas.csproj
```
