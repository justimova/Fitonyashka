using System.Text.Json.Serialization;

namespace Fytonyashka.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    [JsonIgnore]
    public UserDto User { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public DateTime Birthday { get; set; }
    public int Height { get; set; }
}