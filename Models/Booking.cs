namespace FitnessNetworkApp1.Models;

public enum BookingStatus { Active, Cancelled }

public class Booking
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid FitnessClassId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public BookingStatus Status { get; set; } = BookingStatus.Active;
}
