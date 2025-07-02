using System.Text.Json;
using Fytonyashka.DTOs;

namespace Fytonyashka.Services;

public interface IWeightService
{
    bool Create(WeightDto weightDto);
    List<WeightDto> GetAll();
    bool Delete(int id);
    bool Update(WeightDto weightDto);
    WeightDto GetById(int id);
    int? GetLastByUserId(int userId);
    List<WeightDto> GetAllByUserId(int userId);
}

internal class WeightService : IWeightService
{
    private int _nextId = 1;
    private readonly string _dataFilePath;
    private List<WeightDto> _weights = new List<WeightDto>();

    public WeightService() {
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, "Data");
        if (!Directory.Exists(dataDirectory)) {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, "weights.json");

        if (File.Exists(_dataFilePath)) {
            Load();
        }
    }

    private void Load() {
        string weightsJson = File.ReadAllText(_dataFilePath);
        _weights = JsonSerializer.Deserialize<List<WeightDto>>(weightsJson) ?? new List<WeightDto>();
        InitializeNextId();
    }

    private void InitializeNextId() {
        foreach (WeightDto weight in _weights) {
            if (weight.Id >= _nextId) {
                _nextId = weight.Id + 1;
            }
        }
    }

    private void SaveToFile() {
        string weightsJson = JsonSerializer.Serialize(_weights,
            new JsonSerializerOptions { WriteIndented = true }); // pretty print
        File.WriteAllText(_dataFilePath, weightsJson);
    }

    public bool Create(WeightDto weightDto) {
        try {
            var weight = GetAllByUserId(weightDto.UserId)
                .FirstOrDefault(w => w.Date == weightDto.Date);
            if (weight != null) {
                weight.Weight = weightDto.Weight;
            } else {
                weightDto.Id = _nextId++;
                _weights.Add(weightDto);
            }
            SaveToFile();
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public List<WeightDto> GetAll() => _weights.OrderByDescending(w => w.Date).ToList();

    public bool Delete(int id){
        foreach (WeightDto weight in _weights) {
            if (weight.Id == id) {
                _weights.Remove(weight);
                SaveToFile();
                return true;
            }
        }
        return false;
    }

    public bool Update(WeightDto weightDto) {
        var weight = GetById(weightDto.Id);
        if (weight == null) {
            return false;
        }

        weight.Weight = weightDto.Weight;
        SaveToFile();
        return true;
    }

    public WeightDto GetById(int id) {
        foreach (WeightDto weight in _weights) {
            if (weight.Id == id) {
                return weight;
            }
        }
        return null;
    }

    public int? GetLastByUserId(int userId) => (int?)(GetAllByUserId(userId).FirstOrDefault()?.Weight);

    public List<WeightDto> GetAllByUserId(int userId) => GetAll().Where(w => w.UserId == userId).ToList();
}
