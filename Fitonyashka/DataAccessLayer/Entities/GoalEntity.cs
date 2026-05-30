namespace Fitonyashka.DataAccessLayer.Entities;

public class GoalEntity : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal TargetWeight { get; set; }
    public decimal InitialWeight { get; set; }
}
