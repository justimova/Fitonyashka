namespace Fitonyashka.ViewModels.Weight
{
    public record WeightInfoViewModel
    {
        public int Id { get; init; }
        public DateOnly Date { get; init; }
        public decimal Weight { get; init; }
    }
}