
using FitnessNetworkApp.Models;
using FitnessNetworkApp1.Services;

namespace FitnessNetworkApp1.Services;

public class AuthService
{
    private readonly FitnessService _fitness;

    public AuthService(FitnessService fitness)
    {
        _fitness = fitness;
    }

    // ======================
    // REGISTER CLIENT
    // ======================
    public ClientUser RegisterClient(string username, string password)
    {
        username = (username ?? "").Trim();
        password = password ?? "";

        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username invalid.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Parola invalidă.");

        if (_fitness.Users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Username deja există.");

        var user = new ClientUser
        {
            Username = username,
            PasswordHash = password // varianta ușoară (pentru 8 e OK)
        };

        _fitness.Users.Add(user);
        _fitness.SaveAll();
        return user;
    }
    
    // ======================
    // LOGIN
    // ======================
    public User Login(string username, string password)
    {
        username = (username ?? "").Trim();
        password = password ?? "";

        var user = _fitness.Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
            throw new InvalidOperationException("User inexistent.");

        if (user.PasswordHash != password)
            throw new InvalidOperationException("Parolă greșită.");

        return user;
    }
}