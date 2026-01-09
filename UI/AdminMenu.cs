using FitnessNetworkApp1.UI;
using FitnessNetworkApp1.Models;
using FitnessNetworkApp1.Services;

namespace FitnessNetworkApp1.UI;

public static class AdminMenu
{
    public static void Run(FitnessService fitness)
    {
        while (true)
        {
            Console.WriteLine("\n=== ADMIN MENU ===");
            Console.WriteLine("1) Create gym");
            Console.WriteLine("2) Edit gym");
            Console.WriteLine("3) Delete gym");

            Console.WriteLine("4) Add zone");
            Console.WriteLine("5) Edit zone");
            Console.WriteLine("6) Delete zone");

            Console.WriteLine("7) Create plan");
            Console.WriteLine("8) Edit plan");
            Console.WriteLine("9) Delete plan");

            Console.WriteLine("10) Create trainer");

            Console.WriteLine("11) Create class");
            Console.WriteLine("12) Edit class");
            Console.WriteLine("13) Delete class");

            Console.WriteLine("14) View bookings for class");
            Console.WriteLine("15) Stats");

            Console.WriteLine("0) Logout");
            Console.Write("Choose: ");
            var c = Console.ReadLine();

            try
            {
                if (c == "0") return;

                switch (c)
                {
                    case "1":
                        {
                            var name = ConsoleHelpers.ReadNonEmpty("Gym name: ");
                            Console.Write("Address: ");
                            var addr = Console.ReadLine() ?? "";
                            var g = fitness.CreateGym(name, addr);
                            Console.WriteLine($"OK. GymId={g.Id}");
                            break;
                        }
                    case "2":
                        {
                            PrintGyms(fitness);
                            var id = ConsoleHelpers.ReadGuid("GymId: ");
                            var name = ConsoleHelpers.ReadNonEmpty("New name: ");
                            Console.Write("New address: ");
                            var addr = Console.ReadLine() ?? "";
                            fitness.EditGym(id, name, addr);
                            Console.WriteLine("OK.");
                            break;
                        }
                    case "3":
                        {
                            PrintGyms(fitness);
                            var id = ConsoleHelpers.ReadGuid("GymId: ");
                            fitness.DeleteGym(id);
                            Console.WriteLine("OK.");
                            break;
                        }

                    case "4":
                        {
                            PrintGyms(fitness);
                            var gymId = ConsoleHelpers.ReadGuid("GymId: ");
                            var zn = ConsoleHelpers.ReadNonEmpty("Zone name: ");
                            var cap = ConsoleHelpers.ReadInt("Capacity: ");
                            var open = ConsoleHelpers.ReadTimeOnly("Open (HH:mm): ");
                            var close = ConsoleHelpers.ReadTimeOnly("Close (HH:mm): ");
                            var z = fitness.AddZone(gymId, zn, cap, open, close);
                            Console.WriteLine($"OK. ZoneId={z.Id}");
                            break;
                        }
                    case "5":
                        {
                            PrintGyms(fitness);
                            var gymId = ConsoleHelpers.ReadGuid("GymId: ");
                            var gym = fitness.Gyms.Single(g => g.Id == gymId);
                            PrintZones(gym);

                            var zoneId = ConsoleHelpers.ReadGuid("ZoneId: ");
                            var newName = ConsoleHelpers.ReadNonEmpty("New zone name: ");
                            var newCap = ConsoleHelpers.ReadInt("New capacity: ");
                            var open = ConsoleHelpers.ReadTimeOnly("Open (HH:mm): ");
                            var close = ConsoleHelpers.ReadTimeOnly("Close (HH:mm): ");
                            fitness.EditZone(gymId, zoneId, newName, newCap, open, close);
                            Console.WriteLine("OK.");
                            break;
                        }
                    case "6":
                        {
                            PrintGyms(fitness);
                            var gymId = ConsoleHelpers.ReadGuid("GymId: ");
                            var gym = fitness.Gyms.Single(g => g.Id == gymId);
                            PrintZones(gym);

                            var zoneId = ConsoleHelpers.ReadGuid("ZoneId: ");
                            fitness.DeleteZone(gymId, zoneId);
                            Console.WriteLine("OK.");
                            break;
                        }

                    case "7":
                        {
                            var name = ConsoleHelpers.ReadNonEmpty("Plan name: ");
                            var price = ConsoleHelpers.ReadDecimal("Price: ");
                            var days = ConsoleHelpers.ReadInt("Duration days: ");
                            Console.WriteLine("AccessType: 0=Full, 1=ClassesOnly, 2=GymOnly");
                            var type = (AccessType)ConsoleHelpers.ReadInt("Choose: ");
                            var p = fitness.CreatePlan(name, price, days, type);
                            Console.WriteLine($"OK. PlanId={p.Id}");
                            break;
                        }
                    case "8":
                        {
                            PrintPlans(fitness);
                            var id = ConsoleHelpers.ReadGuid("PlanId: ");
                            var name = ConsoleHelpers.ReadNonEmpty("New name: ");
                            var price = ConsoleHelpers.ReadDecimal("New price: ");
                            var days = ConsoleHelpers.ReadInt("New duration days: ");
                            Console.WriteLine("AccessType: 0=Full, 1=ClassesOnly, 2=GymOnly");
                            var type = (AccessType)ConsoleHelpers.ReadInt("Choose: ");
                            fitness.EditPlan(id, name, price, days, type);
                            Console.WriteLine("OK.");
                            break;
                        }
                    case "9":
                        {
                            PrintPlans(fitness);
                            var id = ConsoleHelpers.ReadGuid("PlanId: ");
                            fitness.DeletePlan(id);
                            Console.WriteLine("OK.");
                            break;
                        }

                    case "10":
                        {
                            var n = ConsoleHelpers.ReadNonEmpty("Trainer name: ");
                            var t = fitness.CreateTrainer(n);
                            Console.WriteLine($"OK. TrainerId={t.Id}");
                            break;
                        }

                    case "11":
                        {
                            PrintGyms(fitness);
                            var gymId = ConsoleHelpers.ReadGuid("GymId: ");
                            var gym = fitness.Gyms.Single(g => g.Id == gymId);
                            PrintZones(gym);
                            var zoneId = ConsoleHelpers.ReadGuid("ZoneId: ");

                            PrintTrainers(fitness);
                            var trId = ConsoleHelpers.ReadGuid("TrainerId: ");

                            var type = ConsoleHelpers.ReadNonEmpty("Class type: ");
                            var start = ConsoleHelpers.ReadDateTime("Start (yyyy-MM-dd HH:mm): ");
                            var dur = ConsoleHelpers.ReadInt("Duration minutes: ");
                            var cap = ConsoleHelpers.ReadInt("Capacity: ");

                            var cls = fitness.CreateClass(gymId, zoneId, trId, type, start, dur, cap);
                            Console.WriteLine($"OK. ClassId={cls.Id}");
                            break;
                        }
                    case "12":
                        {
                            PrintClasses(fitness);
                            var classId = ConsoleHelpers.ReadGuid("ClassId: ");

                            var type = ConsoleHelpers.ReadNonEmpty("New type: ");
                            var start = ConsoleHelpers.ReadDateTime("New start (yyyy-MM-dd HH:mm): ");
                            var dur = ConsoleHelpers.ReadInt("New duration minutes: ");
                            var cap = ConsoleHelpers.ReadInt("New capacity: ");

                            PrintTrainers(fitness);
                            var trId = ConsoleHelpers.ReadGuid("TrainerId: ");

                            fitness.EditClass(classId, type, start, dur, cap, trId);
                            Console.WriteLine("OK.");
                            break;
                        }
                    case "13":
                        {
                            PrintClasses(fitness);
                            var classId = ConsoleHelpers.ReadGuid("ClassId: ");
                            fitness.DeleteClass(classId);
                            Console.WriteLine("OK.");
                            break;
                        }

                    case "14":
                        {
                            PrintClasses(fitness);
                            var cid = ConsoleHelpers.ReadGuid("ClassId: ");
                            var list = fitness.GetBookingsForClass(cid);
                            Console.WriteLine($"Bookings: {list.Count}");
                            foreach (var b in list)
                                Console.WriteLine($"- {b.Id} | Client={b.ClientId} | {b.Status} | {b.CreatedAt}");
                            break;
                        }

                    case "15":
                        {
                            var now = DateTime.Now;
                            var stats = fitness.GetStats(now);
                            Console.WriteLine($"Active subscriptions: {stats.ActiveSubs}");
                            foreach (var x in stats.Occupancy)
                                Console.WriteLine($"- {x.Class.Type} @ {x.Class.StartAt:yyyy-MM-dd HH:mm} : {x.Active}/{x.Cap}");
                            break;
                        }

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

    private static void PrintGyms(FitnessService f)
    {
        Console.WriteLine("\nGyms:");
        foreach (var g in f.Gyms)
            Console.WriteLine($"- {g.Id} | {g.Name} | {g.Address} | Zones={g.Zones.Count}");
    }

    private static void PrintZones(Gym g)
    {
        Console.WriteLine("\nZones:");
        foreach (var z in g.Zones)
            Console.WriteLine($"- {z.Id} | {z.Name} | Cap={z.Capacity} | {z.OpenTime}-{z.CloseTime}");
    }

    private static void PrintPlans(FitnessService f)
    {
        Console.WriteLine("\nPlans:");
        foreach (var p in f.Plans)
            Console.WriteLine($"- {p.Id} | {p.Name} | {p.Price} | {p.DurationDays} | {p.AccessType}");
    }

    private static void PrintTrainers(FitnessService f)
    {
        Console.WriteLine("\nTrainers:");
        foreach (var t in f.Trainers)
            Console.WriteLine($"- {t.Id} | {t.FullName}");
    }

    private static void PrintClasses(FitnessService f)
    {
        Console.WriteLine("\nClasses:");
        foreach (var c in f.Classes.OrderBy(x => x.StartAt))
            Console.WriteLine($"- {c.Id} | {c.Type} | {c.StartAt:yyyy-MM-dd HH:mm} | Cap={c.Capacity}");
    }
}
