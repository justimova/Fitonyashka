using Fitonyashka.Models;

namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface IWeightDateRangeService
{
    List<DateRangeModel> GetAll();
    DateRangeModel GetById(int id);
}
