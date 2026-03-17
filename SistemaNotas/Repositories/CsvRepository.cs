using CsvHelper;
using CsvHelper.Configuration;
using SistemaNotas.Models;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SistemaNotas.Repositories;

public class CsvRepository<T> where T : IIdentifiable
{
    private readonly string _filePath;

    public CsvRepository(string entityName)
    {
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");
        
        _filePath = $"Data/{entityName}.csv";
        
        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "");
    }

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

    public void SaveAll(List<T> records)
    {
        using var writer = new StreamWriter(_filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
    }

    public void Create(T entity)
    {
        var records = GetAll();
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        records.Add(entity);
        SaveAll(records);
    }

    public T? Read(Guid id) => GetAll().FirstOrDefault(e => e.Id == id);

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

    public void Delete(Guid id)
    {
        var records = GetAll();
        records.RemoveAll(e => e.Id == id);
        SaveAll(records);
    }
}
