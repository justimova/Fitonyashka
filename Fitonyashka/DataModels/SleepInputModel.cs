using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class SleepInputModel
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateFrom { get; set; }

    public TimeOnly TimeFrom { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateTo { get; set; }

    public TimeOnly TimeTo { get; set; }
}
