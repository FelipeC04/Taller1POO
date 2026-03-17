namespace SistemaNotas.Models;

// HERENCIA: Estudiante hereda de Persona.
public class Estudiante : Persona
{
    public string Matricula { get; set; } = string.Empty;

    // COMPOSICIÓN: Un estudiante tiene su dirección con la cual comparte su ciclo de vida absoluto. (La dirección no es un IIdentifiable independiente, vive dentro del estudiante).
    public Direccion DireccionResidencia { get; set; } = new Direccion();
}
