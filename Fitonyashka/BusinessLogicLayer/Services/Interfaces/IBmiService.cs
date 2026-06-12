namespace Fitonyashka.BusinessLogicLayer.Services.Interfaces;

public interface IBmiService
{
    string GetBmiCategory(decimal bmi);
    decimal CalculateBmi(int height, decimal weight);
    decimal CalculateWeight(int height, decimal bmi);
    IReadOnlyCollection<BmiRange> GetBmiCategories();
}
