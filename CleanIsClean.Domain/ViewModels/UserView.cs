namespace CleanIsClean.Domain.ViewModels;
public partial class UserView
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string RoleName { get; set; } = null!;
}
