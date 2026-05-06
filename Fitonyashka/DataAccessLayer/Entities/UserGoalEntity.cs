namespace Fitonyashka.DataAccessLayer.Entities;

public class UserGoalEntity : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public double Weight { get; set; }
    public double InitialWeight { get; set; }
}
