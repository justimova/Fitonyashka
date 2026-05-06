namespace Fitonyashka.DTOs;

public class SleepDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public double SleepDuration { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
