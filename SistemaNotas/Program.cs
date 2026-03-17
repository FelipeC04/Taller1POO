using System;
using SistemaNotas.Models;
using SistemaNotas.Repositories;

namespace SistemaNotas;

class Program
{
    static CsvRepository<Estudiante> estudiantesDB = new("Estudiantes");
    static CsvRepository<Materia> materiasDB = new("Materias");
    static CsvRepository<Nota> notasDB = new("Notas");

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA EDUCATIVO ===");
            Console.WriteLine("1. Gestión de Estudiantes");
            Console.WriteLine("2. Gestión de Materias");
            Console.WriteLine("3. Gestión de Notas (Asignaciones)");
            Console.WriteLine("4. Salir");
            Console.Write("Opcion: ");
            
            switch (Console.ReadLine())
            {
                case "1": MenuEstudiante(); break;
                case "2": MenuMateria(); break;
                case "3": MenuNota(); break;
                case "4": return;
            }
        }
    }

    static void MenuEstudiante()
    {
        Console.Clear();
        Console.WriteLine("--- CRUD ESTUDIANTE ---");
        Console.WriteLine("1. Crear\n2. Listar\n3. Modificar\n4. Eliminar");
        Console.Write("Opcion: ");
        var opt = Console.ReadLine();
        
        if (opt == "1")
        {
            var est = new Estudiante();
            Console.Write("Nombres: "); est.Nombres = Console.ReadLine() ?? "";
            Console.Write("Apellidos: "); est.Apellidos = Console.ReadLine() ?? "";
            Console.Write("Matricula: "); est.Matricula = Console.ReadLine() ?? "";
            Console.Write("Calle (Direccion): "); est.DireccionResidencia.Calle = Console.ReadLine() ?? "";
            Console.Write("Ciudad (Direccion): "); est.DireccionResidencia.Ciudad = Console.ReadLine() ?? "";
            estudiantesDB.Create(est);
            Console.WriteLine("Guardado correctamente.");
            Console.ReadKey();
        }
        else if (opt == "2")
        {
            var lista = estudiantesDB.GetAll();
            foreach (var e in lista)
                Console.WriteLine($"[{e.Id}] {e.Nombres} {e.Apellidos} - Mat: {e.Matricula} - {e.DireccionResidencia.Ciudad}");
            Console.ReadKey();
        }
        else if (opt == "3")
        {
            Console.Write("ID a modificar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var est = estudiantesDB.Read(id);
                if (est != null)
                {
                    Console.Write($"Nuevo Nombre ({est.Nombres}): ");
                    var n = Console.ReadLine(); if (!string.IsNullOrEmpty(n)) est.Nombres = n;
                    estudiantesDB.Update(est);
                    Console.WriteLine("Actualizado.");
                }
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                estudiantesDB.Delete(id);
                Console.WriteLine("Eliminado.");
            }
            Console.ReadKey();
        }
    }

    static void MenuMateria()
    {
        Console.Clear();
        Console.WriteLine("--- CRUD MATERIA ---");
        Console.WriteLine("1. Crear\n2. Listar\n3. Modificar\n4. Eliminar");
        Console.Write("Opcion: ");
        var opt = Console.ReadLine();
        
        if (opt == "1")
        {
            var mat = new Materia();
            Console.Write("Nombre Materia: "); mat.Nombre = Console.ReadLine() ?? "";
            Console.Write("Creditos: "); mat.Creditos = int.TryParse(Console.ReadLine(), out int c) ? c : 0;
            materiasDB.Create(mat);
            Console.WriteLine("Guardado correctamente.");
            Console.ReadKey();
        }
        else if (opt == "2")
        {
            foreach (var m in materiasDB.GetAll())
                Console.WriteLine($"[{m.Id}] {m.Nombre} - {m.Creditos} creditos");
            Console.ReadKey();
        }
        else if (opt == "3")
        {
            Console.Write("ID a modificar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var mat = materiasDB.Read(id);
                if (mat != null)
                {
                    Console.Write($"Nuevo Nombre ({mat.Nombre}): ");
                    var n = Console.ReadLine(); if (!string.IsNullOrEmpty(n)) mat.Nombre = n;
                    materiasDB.Update(mat);
                    Console.WriteLine("Actualizado.");
                }
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                materiasDB.Delete(id);
                Console.WriteLine("Eliminado.");
            }
            Console.ReadKey();
        }
    }

    static void MenuNota()
    {
        Console.Clear();
        Console.WriteLine("--- CRUD NOTA ---");
        Console.WriteLine("1. Crear (Asociar Estudiante a Materia)\n2. Listar\n3. Modificar\n4. Eliminar");
        Console.Write("Opcion: ");
        var opt = Console.ReadLine();
        
        if (opt == "1")
        {
            var nota = new Nota();
            Console.WriteLine("Estudiantes Disponibles:");
            foreach(var e in estudiantesDB.GetAll()) Console.WriteLine($"{e.Id} -> {e.Nombres} {e.Apellidos}");
            Console.Write("Ingrese ID Estudiante: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid eid)) return;
            nota.EstudianteId = eid;

            Console.WriteLine("\nMaterias Disponibles:");
            foreach(var m in materiasDB.GetAll()) Console.WriteLine($"{m.Id} -> {m.Nombre}");
            Console.Write("Ingrese ID Materia: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid mid)) return;
            nota.MateriaId = mid;

            Console.Write("Valor de la Nota (ej. 4.5): ");
            nota.ValorNota = decimal.TryParse(Console.ReadLine()?.Replace(".", ","), out decimal val) ? val : 0;
            Console.Write("Comentarios: ");
            nota.Comentarios = Console.ReadLine() ?? "";
            
            notasDB.Create(nota);
            Console.WriteLine("Nota registrada correctamente.");
            Console.ReadKey();
        }
        else if (opt == "2")
        {
            var estList = estudiantesDB.GetAll();
            var matList = materiasDB.GetAll();
            foreach (var n in notasDB.GetAll())
            {
                var estName = estList.Find(x => x.Id == n.EstudianteId)?.Nombres ?? "Estudiante Desconocido";
                var matName = matList.Find(x => x.Id == n.MateriaId)?.Nombre ?? "Materia Desconocida";
                Console.WriteLine($"[{n.Id}] Est: {estName} | Mat: {matName} | Nota: {n.ValorNota} ({n.Comentarios})");
            }
            Console.ReadKey();
        }
        else if (opt == "3")
        {
            Console.Write("ID de Nota a modificar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var nota = notasDB.Read(id);
                if (nota != null)
                {
                    Console.Write($"Nueva nota ({nota.ValorNota}): ");
                    var text = Console.ReadLine()?.Replace(".", ",");
                    nota.ValorNota = decimal.TryParse(text, out decimal val) ? val : nota.ValorNota;
                    notasDB.Update(nota);
                    Console.WriteLine("Actualizado.");
                }
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                notasDB.Delete(id);
                Console.WriteLine("Eliminado.");
            }
            Console.ReadKey();
        }
    }
}
