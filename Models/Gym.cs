namespace FitnessNetworkApp1.Models;

public class Gym
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public List<Zone> Zones { get; set; } = new();
}

