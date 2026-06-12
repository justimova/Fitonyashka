namespace Fitonyashka.PresentationLayer.ViewModels.Goal;

public record GoalUpdateViewModel
{
    public int Id { get; init; }
    public decimal InitialWeight { get; init; }
    public decimal TargetWeight { get; init; }
}
