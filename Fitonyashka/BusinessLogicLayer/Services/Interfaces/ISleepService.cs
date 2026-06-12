using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface ISleepService
{
    ResultModel Create(SleepModel sleepDto);
    List<SleepModel> GetAll();
    ResultModel Delete(int id);
    ResultModel Update(SleepModel sleepDto);
    SleepModel GetById(int id);
    SleepModel GetLastByUserId(int userId);
    List<SleepModel> GetAllByUserId(int userId);
}
