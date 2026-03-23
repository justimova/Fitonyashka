namespace Fytonyashka.Core.DateRange;

public interface IDateRangeStrategy
{
    bool IsInRange(DateTime dateToCheck, int filterNumber);
}

