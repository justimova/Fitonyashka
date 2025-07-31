using System.Text.Json;
using Fytonyashka.DTOs;

namespace Fytonyashka.Services;

public interface IWeightDateRangeService
{
    List<DateRangeDto> GetAll();
    DateRangeDto GetById(int id);
}

internal class WeightDateRangeService : IWeightDateRangeService
{
    private int _nextId = 1;
    private readonly string _dataFilePath;
    private List<DateRangeDto> _dateRanges = new List<DateRangeDto>();

    public WeightDateRangeService() {
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, "Data");
        if (!Directory.Exists(dataDirectory)) {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, "weightDateRanges.json");
        if (File.Exists(_dataFilePath)) {
            Load();
        }
    }

    private void Load() {
        string rangesJson = File.ReadAllText(_dataFilePath);
        _dateRanges = JsonSerializer.Deserialize<List<DateRangeDto>>(rangesJson) ?? new List<DateRangeDto>();
        InitializeNextId();
    }

    private void InitializeNextId() {
        foreach (DateRangeDto range in _dateRanges) {
            if (range.Id >= _nextId) {
                _nextId = range.Id + 1;
            }
        }
    }

    public List<DateRangeDto> GetAll() => _dateRanges.OrderBy(w => w.Position).ToList();

    public DateRangeDto GetById(int id) {
        foreach (DateRangeDto range in _dateRanges) {
            if (range.Id == id) {
                return range;
            }
        }
        return null;
    }
}

