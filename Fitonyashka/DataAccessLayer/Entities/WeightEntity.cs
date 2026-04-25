namespace Fytonyashka.DataAccessLayer.Entities;

public class WeightEntity : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateOnly Date { get; set; }
    public decimal Weight { get; set; }
}
