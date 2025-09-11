using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class UserInputModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Minimum length of Username is 3 and maximum is 20")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid format of Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Minimum length of Password is 6 and maximum is 30")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
