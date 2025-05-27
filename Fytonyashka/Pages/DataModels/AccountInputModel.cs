namespace Fytonyashka.Pages.DataModels;

public class AccountInputModel
{
    public int Id { get; set; }
    public UserProfileInputModel User { get; set; }
    public DateTime Birthday { get; set; }
    public string FirstName { get; set; }
    public int Height { get; set; }
}