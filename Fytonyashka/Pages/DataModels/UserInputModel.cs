using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels;
public class UserInputModel : UserProfileInputModel
{
    /*public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }*/

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}