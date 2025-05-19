using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.DataModels;

public class LoginInputModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}