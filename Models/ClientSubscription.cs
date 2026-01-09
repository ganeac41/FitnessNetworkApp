namespace FitnessNetworkApp1.Models;

public class ClientSubscription
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsActive(DateTime now) => now >= StartDate && now <= EndDate;
}
