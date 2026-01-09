namespace FitnessNetworkApp1.Models;

public class FitnessClass
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid GymId { get; set; }
    public Guid ZoneId { get; set; }
    public Guid TrainerId { get; set; }

    public string Type { get; set; } = "";
    public DateTime StartAt { get; set; }
    public int DurationMin { get; set; }
    public int Capacity { get; set; }
}
