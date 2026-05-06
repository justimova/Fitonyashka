namespace Fitonyashka.DTOs;

public class UserGoalDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public double Weight { get; set; }
    public double InitialWeight { get; set; }
}
