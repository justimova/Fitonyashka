using System.Text.Json;
using Fytonyashka.DTOs;

namespace Fytonyashka.Services;

public interface IUserGoalService
{
    UserGoalResultDto Create(UserGoalDto userGoalDto);
    UserGoalResultDto Delete(int userId);
    UserGoalDto GetByUserId(int userId);
}

internal class UserGoalService : IUserGoalService
{
    private int _nextId = 1;
    private readonly string _dataFilePath;
    private List<UserGoalDto> _userGoals = new List<UserGoalDto>();

    public UserGoalService() {
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, "Data");
        if (!Directory.Exists(dataDirectory)) {
            Directory.CreateDirectory(dataDirectory);
        }
        _dataFilePath = Path.Combine(dataDirectory, "userGoals.json");

        if (File.Exists(_dataFilePath)) {
            Load();
        }
    }

    private void Load() {
        string goalsJson = File.ReadAllText(_dataFilePath);
        _userGoals = JsonSerializer.Deserialize<List<UserGoalDto>>(goalsJson) ?? new List<UserGoalDto>();
        InitializeNextId();
    }

    private void InitializeNextId() {
        foreach (UserGoalDto goal in _userGoals) {
            if (goal.Id >= _nextId) {
                _nextId = goal.Id + 1;
            }
        }
    }

    private void SaveToFile() {
        string goalsJson = JsonSerializer.Serialize(_userGoals,
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_dataFilePath, goalsJson);
    }

    private UserGoalResultDto CreateSuccessResult() => new();

    private UserGoalResultDto CreateErrorResult(string errorMessage) =>
        new() {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };

    public UserGoalResultDto Create(UserGoalDto userGoalDto) {
        try {
            userGoalDto.Id = _nextId++;
            _userGoals.Add(userGoalDto);
            SaveToFile();
            return CreateSuccessResult();
        } catch (Exception) {
            return CreateErrorResult("Failed to set goal. Try later or text our support");
        }
    }

    public UserGoalDto GetByUserId(int userId) => _userGoals.FirstOrDefault(u => u.UserId == userId);

    public UserGoalResultDto Delete(int userId) {
        foreach (UserGoalDto goal in _userGoals) {
            if (goal.UserId == userId) {
                _userGoals.Remove(goal);
                SaveToFile();
                return CreateSuccessResult();
            }
        }
        return CreateErrorResult("Failed to delete goal. Try later or text our support");
    }
}
