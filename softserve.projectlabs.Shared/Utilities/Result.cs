namespace softserve.projectlabs.Shared.Utilities;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public T Data { get; set; }
    public int ErrorCode { get; set; }  // Optional error code
    public string StackTrace { get; set; }  // Optional stack trace
    public bool IsNoContent { get; set; }  // Flag to indicate No Content response

    // Success result
    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }

    // Failure result
    public static Result<T> Failure(string errorMessage, int errorCode = 0, string stackTrace = null)
    {
        return new Result<T> { IsSuccess = false, ErrorMessage = errorMessage, ErrorCode = errorCode, StackTrace = stackTrace };
    }

    // No content result (204 status code)
    public static Result<T> NoContent()
    {
        return new Result<T> { IsNoContent = true };
    }
}