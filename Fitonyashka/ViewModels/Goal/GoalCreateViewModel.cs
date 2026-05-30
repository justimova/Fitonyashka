namespace Fitonyashka.ViewModels.Goal;

public record GoalCreateViewModel
{
    public decimal InitialWeight { get; init; }
    public decimal TargetWeight { get; init; }
}
