namespace FitnessNetworkApp1.Models;

public class Trainer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FullName { get; set; } = "";
}
