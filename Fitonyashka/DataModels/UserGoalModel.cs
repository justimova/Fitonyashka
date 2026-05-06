using System.ComponentModel.DataAnnotations;

namespace Fitonyashka.DataModels;

public class UserGoalModel
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date, ErrorMessage = "Invalid value")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Initial weight is required")]
    [Range(1, 600, ErrorMessage = "Please enter a valid weight between 1 and 600 kg")]
    public double InitialWeight { get; set; }

    [Range(1, 600, ErrorMessage = "Please enter a valid weight between 1 and 600 kg")]
    public double Weight { get; set; }
}
