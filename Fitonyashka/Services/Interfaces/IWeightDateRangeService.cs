using Fitonyashka.DTOs;

namespace Fitonyashka.Services.Interfaces;

public interface IWeightDateRangeService
{
    List<DateRangeDto> GetAll();
    DateRangeDto GetById(int id);
}
