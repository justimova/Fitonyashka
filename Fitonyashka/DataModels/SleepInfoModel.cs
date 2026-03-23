namespace Fytonyashka.DataModels;

public class SleepInfoModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string SleepDuration { get; set; }
}
