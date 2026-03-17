using System;
using SistemaNotas.Models;
using SistemaNotas.Repositories;

namespace SistemaNotas
{
    class Program
    {
// Clase principal del sistema educativo. Gestiona el menú principal y las operaciones CRUD para estudiantes, materias y notas.
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
            Console.Write("Nombres: ");
            est.Nombres = LeerTextoNoNumerico();
            Console.Write("Apellidos: ");
            est.Apellidos = LeerTextoNoNumerico();
            Console.Write("Matricula: ");
            est.Matricula = LeerTextoNoVacio();
            Console.Write("Calle (Direccion): ");
            est.DireccionResidencia.Calle = LeerTextoNoVacio();
            Console.Write("Ciudad (Direccion): ");
            est.DireccionResidencia.Ciudad = LeerTextoNoVacio();
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
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            var est = estudiantesDB.Read(id);
            if (est != null)
            {
                Console.Write($"Nuevo Nombre ({est.Nombres}): ");
                var n = Console.ReadLine(); if (!string.IsNullOrEmpty(n)) est.Nombres = n;
                estudiantesDB.Update(est);
                Console.WriteLine("Actualizado.");
            }
            else
            {
                Console.WriteLine("No se encontró un estudiante con ese ID.");
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            estudiantesDB.Delete(id);
            Console.WriteLine("Eliminado.");
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
            Console.Write("Nombre Materia: "); mat.Nombre = LeerTextoNoNumerico();
            Console.Write("Creditos: "); mat.Creditos = LeerEnteroPositivo();
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
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            var mat = materiasDB.Read(id);
            if (mat != null)
            {
                Console.Write($"Nuevo Nombre ({mat.Nombre}): ");
                var n = Console.ReadLine(); if (!string.IsNullOrEmpty(n)) mat.Nombre = n;
                materiasDB.Update(mat);
                Console.WriteLine("Actualizado.");
            }
            else
            {
                Console.WriteLine("No se encontró una materia con ese ID.");
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            materiasDB.Delete(id);
            Console.WriteLine("Eliminado.");
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
            var inputEst = Console.ReadLine();
            if (!int.TryParse(inputEst, out int eid))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido para el ID de estudiante.");
                Console.ReadKey();
                return;
            }
            nota.EstudianteId = eid;

            Console.WriteLine("\nMaterias Disponibles:");
            foreach(var m in materiasDB.GetAll()) Console.WriteLine($"{m.Id} -> {m.Nombre}");
            Console.Write("Ingrese ID Materia: ");
            var inputMat = Console.ReadLine();
            if (!int.TryParse(inputMat, out int mid))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido para el ID de materia.");
                Console.ReadKey();
                return;
            }
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
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            var nota = notasDB.Read(id);
            if (nota != null)
            {
                Console.Write($"Nueva nota ({nota.ValorNota}): ");
                var text = Console.ReadLine()?.Replace(".", ",");
                nota.ValorNota = decimal.TryParse(text, out decimal val) ? val : nota.ValorNota;
                notasDB.Update(nota);
                Console.WriteLine("Actualizado.");
            }
            else
            {
                Console.WriteLine("No se encontró una nota con ese ID.");
            }
            Console.ReadKey();
        }
        else if (opt == "4")
        {
            Console.Write("ID a eliminar: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Error: Debe ingresar un número entero válido.");
                Console.ReadKey();
                return;
            }
            notasDB.Delete(id);
            Console.WriteLine("Eliminado.");
            Console.ReadKey();
        }
    }

    // Métodos de validación de entrada (solo una vez, al final de la clase)
    static string LeerTextoNoNumerico()
    {
        while (true)
        {
            var input = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("El valor no puede estar vacío. Intente de nuevo: ");
                continue;
            }
            if (int.TryParse(input, out _))
            {
                Console.Write("No puede ser solo números. Intente de nuevo: ");
                continue;
            }
            return input;
        }
    }

    static string LeerTextoNoVacio()
    {
        while (true)
        {
            var input = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("El valor no puede estar vacío. Intente de nuevo: ");
                continue;
            }
            return input;
        }
    }

    static int LeerEnteroPositivo()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int valor) || valor <= 0)
            {
                Console.Write("Debe ingresar un número entero positivo. Intente de nuevo: ");
                continue;
            }
            return valor;
        }
    }
    }
}
