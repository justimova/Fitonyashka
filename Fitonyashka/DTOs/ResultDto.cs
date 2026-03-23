namespace Fytonyashka.DTOs;

public class ResultDto
{
    public bool IsSuccess { get; set; } = true;
    public string ErrorMessage { get; set; }

    public static ResultDto CreateSuccessResult() => new ResultDto();

    public static ResultDto CreateFailedResult(string errorMessage) => new ResultDto {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
