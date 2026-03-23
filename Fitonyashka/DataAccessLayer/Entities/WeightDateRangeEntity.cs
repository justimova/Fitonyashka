namespace Fytonyashka.DataAccessLayer.Entities;

public class WeightDateRangeEntity : IIdentifiable
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsFiltered { get; set; }
    public int FilterNumber { get; set; }
    public int FilterDateRange { get; set; }
    public int Position { get; set; }
}
