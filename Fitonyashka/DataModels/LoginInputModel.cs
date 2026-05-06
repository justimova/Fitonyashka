using System.ComponentModel.DataAnnotations;

namespace Fitonyashka.DataModels;

public class LoginInputModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
