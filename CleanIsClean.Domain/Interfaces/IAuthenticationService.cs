using CleanIsClean.Domain.ViewModels;

namespace CleanIsClean.Domain.Interfaces;
public interface IAuthenticationService
{
    Task<string?> Login(LoginView loginView);
}
