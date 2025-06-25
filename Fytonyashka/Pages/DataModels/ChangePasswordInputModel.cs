using System.ComponentModel.DataAnnotations;

namespace Fytonyashka.Pages.DataModels
{
	public class ChangePasswordInputModel
	{
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}

