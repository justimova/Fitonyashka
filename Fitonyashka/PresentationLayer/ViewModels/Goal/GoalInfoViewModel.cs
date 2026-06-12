namespace Fitonyashka.PresentationLayer.ViewModels.Goal;

public record GoalInfoViewModel
{
    public int Id { get; init; }
    public DateTime StartDate { get; init; }
    public decimal InitialWeight { get; init; }
    public decimal TargetWeight { get; init; }
}
