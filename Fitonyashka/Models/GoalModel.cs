namespace Fitonyashka.Models;

public class GoalModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public decimal TargetWeight { get; set; }
    public decimal InitialWeight { get; set; }
}
