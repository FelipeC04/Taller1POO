using System;

namespace SistemaNotas.Models;

// INTERFAZ COMPLEMENTADA
public interface IIdentifiable 
{ 
    Guid Id { get; set; } 
}
