namespace Fitonyashka.DataAccessLayer.Entities;

public class UserEntity : IIdentifiable
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    public int Gender { get; set; }
    public int Height { get; set; }
    public string AvatarFileName { get; set; }
    public int SelectedDateRangeId { get; set; }
}
