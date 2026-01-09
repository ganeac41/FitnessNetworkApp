namespace FitnessNetworkApp1.Models;

public class Zone
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public int Capacity { get; set; }
    public TimeOnly OpenTime { get; set; } = new(8, 0);
    public TimeOnly CloseTime { get; set; } = new(22, 0);
}
