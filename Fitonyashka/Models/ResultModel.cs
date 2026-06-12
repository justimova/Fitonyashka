namespace Fitonyashka.Models;

public class ResultModel
{
    public bool IsSuccess { get; set; } = true;
    public string ErrorMessage { get; set; }

    public static ResultModel CreateSuccessResult() => new ResultModel();

    public static ResultModel CreateFailedResult(string errorMessage) => new ResultModel
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
