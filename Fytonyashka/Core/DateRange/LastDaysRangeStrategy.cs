namespace Fytonyashka.Core.DateRange;

public class LastDaysRangeStrategy : IDateRangeStrategy
{
    public bool IsInRange(DateTime dateToCheck, int filterNumber) =>
		dateToCheck.IsWithinLastDays(filterNumber);
}
