namespace Fitonyashka.PresentationLayer.ViewModels.Weight;

public record WeightBaseViewModel
{
    public DateOnly Date { get; init; }
    public decimal Weight { get; init; }
}
