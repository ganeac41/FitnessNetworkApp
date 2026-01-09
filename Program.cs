using FitnessNetworkApp1.Models;
using FitnessNetworkApp1.Services;
using FitnessNetworkApp1.Storage;
using FitnessNetworkApp1.UI;

var storage = new JsonFileStorage("Data");
var fitness = new FitnessService(storage);
var auth = new AuthService(fitness);

while (true)
{
    Console.WriteLine("\n=== FITNESS NETWORK ===");
    Console.WriteLine("1) Login");
    Console.WriteLine("2) Register client");
    Console.WriteLine("0) Exit");
    Console.Write("Choose: ");
    var c = Console.ReadLine();

    try
    {
        if (c == "0")
            return;

        if (c == "2")
        {
            var u = ConsoleHelpers.ReadNonEmpty("Username: ");
            Console.Write("Password: ");
            var p = Console.ReadLine() ?? "";

            var client = auth.RegisterClient(u, p);
            Console.WriteLine($"OK. Client created: {client.Username}");
        }
        else if (c == "1")
        {
            var u = ConsoleHelpers.ReadNonEmpty("Username: ");
            Console.Write("Password: ");
            var p = Console.ReadLine() ?? "";

            var user = auth.Login(u, p);
            Console.WriteLine($"Logged in as: {user.Role}");

            if (user.Role == Role.Admin)
                AdminMenu.Run(fitness);
            else
                ClientMenu.Run(fitness, user.Id);
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}