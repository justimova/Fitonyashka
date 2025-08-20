namespace Fytonyashka.DTOs;

public class BaseResultDto
{
    public bool IsSuccess { get; set; } = true;
    public string ErrorMessage { get; set; }
}

public class UserSaveResultDto : BaseResultDto
{
}

public class UserGoalResultDto : BaseResultDto
{
}