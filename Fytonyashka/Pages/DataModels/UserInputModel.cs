using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels;
public class UserInputModel : UserProfileInputModel
{
    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}