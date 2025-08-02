using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels;
public class WeightInputModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Weight is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
    public double Weight { get; set; }
}
