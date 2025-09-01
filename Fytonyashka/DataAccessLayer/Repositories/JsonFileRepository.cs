using System.Text.Json;
using Fytonyashka.DataAccessLayer.Entities;
using Fytonyashka.DTOs;

namespace Fytonyashka.DataAccessLayer.Repositories;

internal abstract class JsonFileRepository<TEntity> where TEntity: class, IIdentifiable
{
    private int _nextId = 1;
    private readonly string _dataFilePath;
    private List<TEntity> _entities = new();

    public JsonFileRepository() {
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, "Data");
        if (!Directory.Exists(dataDirectory)) {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, $"{GetEntityName()}s.json");

        if (File.Exists(_dataFilePath)) {
            Load();
        }
    }

    public List<TEntity> GetAll() => _entities;

    public void Add(TEntity entity) {
        entity.Id = _nextId++;
        _entities.Add(entity);
        SaveToFile();
    }

    public void Update(TEntity entity) {
        var index = _entities.FindIndex(e => e.Id == entity.Id);
        if (index < 0) {
            return;
        }
        _entities[index] = entity;
        SaveToFile();
    }

    public void Delete(int id) {
        var index = _entities.FindIndex(e => e.Id == id);
        if (index < 0) {
            return;
        }
        _entities.RemoveAt(index);
        SaveToFile();
    }

    protected abstract string GetEntityName();

    private void Load() {
        string entitiesJson = File.ReadAllText(_dataFilePath);
        _entities = JsonSerializer.Deserialize<List<TEntity>>(entitiesJson) ?? new List<TEntity> ();
        InitializeNextId();
    }

    private void InitializeNextId() {
        foreach (TEntity entity in _entities) {
            if (entity.Id >= _nextId) {
                _nextId = entity.Id + 1;
            }
        }
    }

    private void SaveToFile() {
        string entitiesJson = JsonSerializer.Serialize(_entities,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_dataFilePath, entitiesJson);
    }
}
