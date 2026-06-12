namespace Fitonyashka.BusinessLogicLayer.Core.DateRange;

public static class DateRangeStrategyFactory
{
    public static IDateRangeStrategy GetStrategy(DateRangeOption option) {
        return option switch {
            DateRangeOption.LastNDays => new LastDaysRangeStrategy(),
            DateRangeOption.LastNMonths => new LastMonthsRangeStrategy(),
            DateRangeOption.LastNYears => new LastYearsRangeStrategy(),
            _ => throw new NotSupportedException("Unsupported range option")
        };
    }
}
