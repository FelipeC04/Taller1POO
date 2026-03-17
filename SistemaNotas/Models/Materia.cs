using System;

namespace SistemaNotas.Models;

public class Materia : IIdentifiable
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }

    // AGREGACIÓN: Una materia 'tiene' un Profesor titular asignado, pero el profesor existe independientemente de la Materia (por eso ligamos relacion por el Id y no le damos ciclo de vida a la instancia interna).
    public Guid? ProfesorTitularId { get; set; }
}
