namespace Fitonyashka.DTOs;

public interface IUserDto
{
    string UserName { get; set; }
    int Id { get; set; }
    string Email { get; set; }
}

public class UserDto : UserProfileDto
{
    public string Password { get; set; }
}

public class UserProfileDto : IUserDto
{
    public string UserName { get; set; }
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    public int Gender { get; set; }
    public int Height { get; set; }
    public string AvatarFileName { get; set; }
    public int SelectedDateRangeId { get; set; } = 2;
}
