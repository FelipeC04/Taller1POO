using CsvHelper;
using CsvHelper.Configuration;
using SistemaNotas.Models;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

// Repositorio genérico para operaciones CRUD sobre entidades que implementan IIdentifiable, usando archivos CSV como almacenamiento.
// T: Tipo de entidad que implementa IIdentifiable.
namespace SistemaNotas.Repositories;

// Clase genérica para operaciones CRUD sobre entidades que implementan IIdentifiable, usando archivos CSV como almacenamiento.
public class CsvRepository<T> where T : IIdentifiable
{
    // Ruta al archivo CSV donde se almacenan los datos de la entidad.
    private readonly string _filePath;

    // Inicializa el repositorio, asegurando la existencia de la carpeta y archivo CSV correspondiente.
    // entityName: Nombre de la entidad (usado para nombrar el archivo CSV).
    public CsvRepository(string entityName)
    {
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");
        _filePath = $"Data/{entityName}.csv";
        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "");
    }

    // Obtiene todas las entidades almacenadas en el archivo CSV.
    // Retorna una lista de entidades de tipo T.
    public List<T> GetAll()
    {
        if (new FileInfo(_filePath).Length == 0) return new List<T>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
        };
        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, config);
        return csv.GetRecords<T>().ToList();
    }

    // Guarda todas las entidades proporcionadas en el archivo CSV, sobrescribiendo el contenido anterior.
    // records: Lista de entidades a guardar.
    public void SaveAll(List<T> records)
    {
        using var writer = new StreamWriter(_filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
    }

    // Agrega una nueva entidad al archivo CSV. Si el Id es 0, se asigna un nuevo Id incremental.
    // entity: Entidad a agregar.
    public void Create(T entity)
    {
        var records = GetAll();
        if (entity.Id == 0)
        {
            entity.Id = records.Count > 0 ? records.Max(e => e.Id) + 1 : 1;
        }
        records.Add(entity);
        SaveAll(records);
    }

    // Busca y retorna una entidad por su Id.
    // id: Identificador único de la entidad.
    // Retorna la entidad encontrada o null si no existe.
    public T? Read(int id) => GetAll().FirstOrDefault(e => e.Id == id);

    // Actualiza una entidad existente en el archivo CSV.
    // entity: Entidad con los datos actualizados.
    public void Update(T entity)
    {
        var records = GetAll();
        var index = records.FindIndex(e => e.Id == entity.Id);
        if (index >= 0)
        {
            records[index] = entity;
            SaveAll(records);
        }
    }

    // Elimina una entidad del archivo CSV por su Id.
    // id: Identificador único de la entidad a eliminar.
    public void Delete(int id)
    {
        var records = GetAll();
        records.RemoveAll(e => e.Id == id);
        SaveAll(records);
    }
}
