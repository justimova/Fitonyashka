using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class ChangePasswordInputModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Minimum length of Password is 6 and maximum is 30")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}
