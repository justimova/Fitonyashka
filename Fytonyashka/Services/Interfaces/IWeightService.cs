using Fytonyashka.DTOs;

namespace Fytonyashka.Services.Interfaces;

public interface IWeightService
{
    bool Create(WeightDto weightDto);
    List<WeightDto> GetAll();
    bool Delete(int id);
    bool Update(WeightDto weightDto);
    WeightDto GetById(int id);
    WeightDto GetLastByUserId(int userId);
    List<WeightDto> GetAllByUserId(int userId);
}
