namespace Fitonyashka.PresentationLayer.ViewModels.UserProfile;

public class UserUpdateViewModel
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public DateOnly Birthday { get; set; }
    public int Gender { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
}
