using FitnessNetworkApp1.Models;

namespace FitnessNetworkApp.Models;

public abstract class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; } = "";
    public string PasswordHash { get; init; } = "";
    public Role Role { get; init; }

    protected User(Role role) => Role = role;
}

public sealed class AdminUser : User
{
    public AdminUser() : base(Role.Admin) { }
}

public sealed class ClientUser : User
{
    public ClientUser() : base(Role.Client) { }
}