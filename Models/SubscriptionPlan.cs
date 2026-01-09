namespace FitnessNetworkApp1.Models;
public enum AccessType { Full, ClassesOnly, GymOnly }

public class SubscriptionPlan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public AccessType AccessType { get; set; } = AccessType.Full;
}
