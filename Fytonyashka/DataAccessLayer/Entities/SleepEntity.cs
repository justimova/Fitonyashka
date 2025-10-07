namespace Fytonyashka.DataAccessLayer.Entities;

public class SleepEntity : IIdentifiable
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
