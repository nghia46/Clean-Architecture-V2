namespace CleanIsClean.Domain.Interfaces;
public interface IAuthenticationService
{
    Task<string?> Login(string email, string password);
}
