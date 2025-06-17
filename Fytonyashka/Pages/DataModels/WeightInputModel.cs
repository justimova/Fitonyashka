using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels;
public class WeightInputModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required]
    public double Weight { get; set; }
}