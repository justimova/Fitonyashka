namespace Fitonyashka.BusinessLogicLayer.Core.DateRange;

public class LastMonthsRangeStrategy : IDateRangeStrategy
{
    public bool IsInRange(DateTime dateToCheck, int filterNumber) =>
        dateToCheck.IsWithinLastMonths(filterNumber);
}
