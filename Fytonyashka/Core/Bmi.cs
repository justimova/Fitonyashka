namespace Fytonyashka.Core;

public class BmiRange
{
    public double Min { get; set; }
    public double Max { get; set; }
    public string Category { get; set; }
}

public class Bmi
{
    private static readonly List<BmiRange> _bmiRanges = new() {
        new BmiRange { Min = 0, Max = 16, Category = "Severe underweight" }, 
        new BmiRange { Min = 16, Max = 18.5, Category = "Underweight" },
        new BmiRange { Min = 18.5, Max = 25, Category = "Normal" },
        new BmiRange { Min = 25, Max = 30, Category = "Overweight" },
        new BmiRange { Min = 30, Max = 35, Category = "Obesity class I" },
        new BmiRange { Min = 35, Max = 40, Category = "Obesity class II" },
        new BmiRange { Min = 40, Max = double.MaxValue, Category = "Obesity class III" }
    };

    public static List<BmiRange> BmiRanges => _bmiRanges;

    public int Height { get; set; } = 0;
    public double Weight { get; set; } = 0;
    public double BmiValue {
        get => Height <= 0 || Weight <= 0 ? 0 : (Weight * 10000) / (Height * Height); 
    }
    public string BmiCategory {
        get => _bmiRanges.FirstOrDefault(r => BmiValue >= r.Min && BmiValue < r.Max)?.Category ?? "Unknown";
    }
}
