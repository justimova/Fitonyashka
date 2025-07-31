namespace Fytonyashka.Core;

public static class DataTimeExtensions
{
	public static bool IsWithinLastDays(this DateTime source, int days) {
        DateTime now = DateTime.Now;
        return source >= now.AddDays(-days) && source <= now;
    }

    public static bool IsWithinLastMonths(this DateTime source, int months) {
        DateTime now = DateTime.Now;
        return source >= now.AddMonths(-months) && source <= now;
    }

    public static bool IsWithinLastYears(this DateTime source, int years) {
        DateTime now = DateTime.Now;
        return source >= now.AddYears(-years) && source <= now;
    }
}
