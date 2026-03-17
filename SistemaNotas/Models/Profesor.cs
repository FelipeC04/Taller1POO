namespace SistemaNotas.Models;

// HERENCIA : Profesor hereda de Persona
public class Profesor : Persona
{
    public string Especialidad { get; set; } = string.Empty;
}
