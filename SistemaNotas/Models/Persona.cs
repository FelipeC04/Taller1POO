using System;

namespace SistemaNotas.Models;

// HERENCIA - Clase abstracta base
public abstract class Persona : IIdentifiable
{
    public Guid Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
}
