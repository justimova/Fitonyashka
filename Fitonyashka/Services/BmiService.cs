using Fitonyashka.Services.Interfaces;

namespace Fitonyashka.Services;

public class BmiRange
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public string Category { get; set; }
}

public class BmiService : IBmiService
{
    private static List<BmiRange> BmiRanges => new() {
        new BmiRange { Min = 0, Max = 16, Category = "Severe underweight" },
        new BmiRange { Min = 16, Max = 18.5M, Category = "Underweight" },
        new BmiRange { Min = 18.5M, Max = 25, Category = "Normal" },
        new BmiRange { Min = 25, Max = 30, Category = "Overweight" },
        new BmiRange { Min = 30, Max = 35, Category = "Obesity class I" },
        new BmiRange { Min = 35, Max = 40, Category = "Obesity class II" },
        new BmiRange { Min = 40, Max = decimal.MaxValue, Category = "Obesity class III" }
    };

    public string GetBmiCategory(decimal bmi) =>
        BmiRanges.FirstOrDefault(r => bmi >= r.Min && bmi < r.Max)?.Category ?? string.Empty;

    public IReadOnlyCollection<BmiRange> GetBmiCategories() => BmiRanges;

    public decimal CalculateBmi(int height, decimal weight) =>
        height <= 0 || weight <= 0 ? 0 : weight * 10000 / (height * height);

    public decimal CalculateWeight(int height, decimal bmi) =>
        height <= 0 || bmi <= 0 ? 0 : bmi * height * height / 10000;
}
