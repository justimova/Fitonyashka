namespace Fitonyashka.Models;

public class WeightModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly Date { get; set; }
    public decimal Weight { get; set; }
}
