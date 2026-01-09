using FitnessNetworkApp1.Models;
using FitnessNetworkApp1.Services;

namespace FitnessNetworkApp1.UI;

public static class ClientMenu
{
    public static void Run(FitnessService fitness, Guid clientId)
    {
        while (true)
        {
            Console.WriteLine("\n=== CLIENT MENU ===");
            Console.WriteLine("1) View gyms");
            Console.WriteLine("2) View subscription plans");
            Console.WriteLine("3) Buy subscription");
            Console.WriteLine("4) View classes");
            Console.WriteLine("5) Reserve class");
            Console.WriteLine("6) My activity");
            Console.WriteLine("7) Cancel booking");
            Console.WriteLine("0) Logout");
            Console.Write("Choose: ");

            var c = Console.ReadLine();

            try
            {
                if (c == "0")
                    return;

                switch (c)
                {
                    case "1":
                        PrintGyms(fitness);
                        break;

                    case "2":
                        PrintPlans(fitness);
                        break;

                    case "3":
                        PrintPlans(fitness);
                        var planId = ConsoleHelpers.ReadGuid("PlanId: ");
                        var sub = fitness.BuySubscription(clientId, planId, DateTime.Now);
                        Console.WriteLine($"Subscription active until {sub.EndDate:yyyy-MM-dd}");
                        break;

                    case "4":
                        PrintClasses(fitness);
                        break;

                    case "5":
                        PrintClasses(fitness);
                        var classId = ConsoleHelpers.ReadGuid("ClassId: ");
                        var booking = fitness.ReserveClass(clientId, classId, DateTime.Now);
                        Console.WriteLine($"Reserved. BookingId={booking.Id}");
                        break;

                    case "6":
                        ShowMyActivity(fitness, clientId);
                        break;

                    case "7":
                        CancelBooking(fitness, clientId);
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    // ================= Helpers =================

    private static void PrintGyms(FitnessService f)
    {
        Console.WriteLine("\nGyms:");
        if (f.Gyms.Count == 0)
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var g in f.Gyms)
            Console.WriteLine($"- {g.Id} | {g.Name} | {g.Address}");
    }

    private static void PrintPlans(FitnessService f)
    {
        Console.WriteLine("\nPlans:");
        if (f.Plans.Count == 0)
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var p in f.Plans)
            Console.WriteLine($"- {p.Id} | {p.Name} | {p.Price} | {p.DurationDays} days | {p.AccessType}");
    }

    private static void PrintClasses(FitnessService f)
    {
        Console.WriteLine("\nClasses:");
        if (f.Classes.Count == 0)
        {
            Console.WriteLine("(none)");
            return;
        }

        foreach (var c in f.Classes.OrderBy(x => x.StartAt))
        {
            var active = f.GetActiveBookingsCount(c.Id);
            Console.WriteLine($"- {c.Id} | {c.Type} | {c.StartAt:yyyy-MM-dd HH:mm} | {active}/{c.Capacity}");
        }
    }

    private static void ShowMyActivity(FitnessService f, Guid clientId)
    {
        Console.WriteLine("\n-- Subscriptions --");
        var subs = f.GetClientSubscriptions(clientId);
        if (subs.Count == 0) Console.WriteLine("(none)");
        foreach (var s in subs)
            Console.WriteLine($"- {s.StartDate:yyyy-MM-dd} → {s.EndDate:yyyy-MM-dd} | Active={s.IsActive(DateTime.Now)}");

        Console.WriteLine("\n-- Bookings --");
        var books = f.GetClientBookings(clientId);
        if (books.Count == 0) Console.WriteLine("(none)");
        foreach (var b in books)
        {
            var cls = f.Classes.FirstOrDefault(x => x.Id == b.FitnessClassId);
            var label = cls == null ? "class?" : $"{cls.Type} @ {cls.StartAt:yyyy-MM-dd HH:mm}";
            Console.WriteLine($"- {b.Id} | {label} | {b.Status}");
        }
    }

    private static void CancelBooking(FitnessService f, Guid clientId)
    {
        var active = f.GetClientBookings(clientId)
            .Where(b => b.Status == BookingStatus.Active)
            .ToList();

        if (active.Count == 0)
        {
            Console.WriteLine("No active bookings.");
            return;
        }

        Console.WriteLine("\nActive bookings:");
        foreach (var b in active)
        {
            var cls = f.Classes.FirstOrDefault(x => x.Id == b.FitnessClassId);
            var label = cls == null ? "class?" : $"{cls.Type} @ {cls.StartAt:yyyy-MM-dd HH:mm}";
            Console.WriteLine($"- {b.Id} | {label}");
        }

        var bid = ConsoleHelpers.ReadGuid("BookingId: ");
        f.CancelBooking(clientId, bid, DateTime.Now, minHoursBefore: 2);
        Console.WriteLine("Cancelled.");
    }
}
