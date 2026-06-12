namespace Fitonyashka.Models;

public class DateRangeModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsFiltered { get; set; }
    public int FilterNumber { get; set; }
    public int FilterDateRange { get; set; }
    public int Position { get; set; }
}
