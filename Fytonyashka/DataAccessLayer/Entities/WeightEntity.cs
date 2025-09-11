namespace Fytonyashka.DataAccessLayer.Entities;

public class WeightEntity : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public double Weight { get; set; }
}
