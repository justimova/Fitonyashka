namespace Fytonyashka.Core.DateRange;

public class LastYearsRangeStrategy : IDateRangeStrategy
{
    public bool IsInRange(DateTime dateToCheck, int filterNumber) =>
        dateToCheck.IsWithinLastYears(filterNumber);
}
