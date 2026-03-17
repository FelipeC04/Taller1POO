using System;

namespace SistemaNotas.Models;

// ASOCIACIÓN: Esta clase existe como puente de asociación entre un Estudiante y una Materia. 
public class Nota : IIdentifiable
{
    public Guid Id { get; set; }

    public Guid EstudianteId { get; set; }
    public Guid MateriaId { get; set; }

    public decimal ValorNota { get; set; }
    public string Comentarios { get; set; } = string.Empty;
}
