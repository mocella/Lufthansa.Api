namespace Lufthansa.Api.Models.Shared;

public class ApiResponse<TSuccess, TError>
{
    public TSuccess? Success { get; set; }
    public TError? Error { get; set; }
    public bool IsSuccess { get; set; }

    public static ApiResponse<TSuccess, TError> FromSuccess(TSuccess success)
    {
        return new ApiResponse<TSuccess, TError> { Success = success, IsSuccess = true };
    }

    public static ApiResponse<TSuccess, TError> FromError(TError error)
    {
        return new ApiResponse<TSuccess, TError> { Error = error, IsSuccess = false };
    }
}