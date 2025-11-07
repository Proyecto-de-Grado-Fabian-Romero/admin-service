namespace AdminService.Src.Application.Interfaces;

public interface IResponseListener
{
    Task<T?> WaitForResponse<T>(string correlationId, TimeSpan timeout);

    void RegisterResponse<T>(string correlationId, T response);
}
