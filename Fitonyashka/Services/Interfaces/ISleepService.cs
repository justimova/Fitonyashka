using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface ISleepService
{
    ResultDto Create(SleepDto sleepDto);
    List<SleepDto> GetAll();
    ResultDto Delete(int id);
    ResultDto Update(SleepDto sleepDto);
    SleepDto GetById(int id);
    SleepDto GetLastByUserId(int userId);
    List<SleepDto> GetAllByUserId(int userId);
}
