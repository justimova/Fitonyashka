using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface IWeightService
{
    ResultDto Create(WeightDto weightDto);
    List<WeightDto> GetAll();
    ResultDto Delete(int id);
    ResultDto Update(WeightDto weightDto);
    WeightDto GetById(int id);
    WeightDto GetLastByUserId(int userId);
    List<WeightDto> GetAllByUserId(int userId);
}
