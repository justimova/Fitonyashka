namespace Fitonyashka.ViewModels.UserProfile;

public class UserInfoViewModel
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public DateOnly Birthday { get; set; }
    public int Gender { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string AvatarFileName { get; set; }
    public int SelectedDateRangeId { get; set; } = 2;
}

