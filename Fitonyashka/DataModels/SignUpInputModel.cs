using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class SignUpInputModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName {
        get; set;
    }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid format of Email")]
    public string Email {
        get; set;
    }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password {
        get; set;
    }
}
