using Fytonyashka.DTOs;

namespace Fytonyashka.Services.Interfaces;

public interface IWeightDateRangeService
{
    List<DateRangeDto> GetAll();
    DateRangeDto GetById(int id);
}
