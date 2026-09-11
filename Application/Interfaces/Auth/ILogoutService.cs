namespace Application.Interfaces.Auth
{
    public interface ILogoutService
    {
        Task LogoutAsync(CancellationToken cancellationToken);
    }
}
