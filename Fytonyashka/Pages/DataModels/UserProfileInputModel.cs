using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels;

public class UserProfileInputModel
{
    public int Id { get; set; }

    public DateTime Birthday { get; set; } = DateTime.UtcNow;

    public int Gender { get; set; } = 0;

    public string FirstName { get; set; } = "";

    public int Height { get; set; } = 0;

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string? AvatarFileName { get; set; }
}