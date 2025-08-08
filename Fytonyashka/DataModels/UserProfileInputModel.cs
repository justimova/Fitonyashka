using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class UserProfileInputModel
{
    public int Id { get; set; }

    public DateTime Birthday { get; set; } = DateTime.UtcNow;

    public int Gender { get; set; } = 0;

    public string FirstName { get; set; } = "";

    [Range(0.01, double.MaxValue, ErrorMessage = "Height must be greater than 0")]
    public int Height { get; set; } = 0;

    public double? Weight { get; set; } = 0;

    [Required(ErrorMessage = "Username is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Minimum length of Username is 3 and maximum is 20")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid format of Email")]
    public string Email { get; set; }

    public string? AvatarFileName { get; set; }
}
