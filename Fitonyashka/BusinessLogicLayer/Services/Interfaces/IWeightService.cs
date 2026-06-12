using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface IWeightService
{
    ResultModel Create(WeightModel weightDto);
    List<WeightModel> GetAll();
    ResultModel Delete(int id);
    ResultModel Update(WeightModel weightDto);
    WeightModel GetById(int id);
    WeightModel GetLastByUserId(int userId);
    List<WeightModel> GetAllByUserId(int userId);
}
