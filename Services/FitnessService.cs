using FitnessNetworkApp.Models;
using FitnessNetworkApp1.Models;
using FitnessNetworkApp1.Storage;

namespace FitnessNetworkApp1.Services;

public class FitnessService
{
    private readonly JsonFileStorage _storage;

    // "Baza de date" în memorie
    public List<User> Users { get; private set; } = new();
    public List<Gym> Gyms { get; private set; } = new();
    public List<SubscriptionPlan> Plans { get; private set; } = new();
    public List<Trainer> Trainers { get; private set; } = new();
    public List<FitnessClass> Classes { get; private set; } = new();
    public List<ClientSubscription> Subscriptions { get; private set; } = new();
    public List<Booking> Bookings { get; private set; } = new();

    public FitnessService(JsonFileStorage storage)
    {
        _storage = storage;
        LoadAll();
        EnsureAdmin();
    }

    public void LoadAll()
    {
        Users = _storage.Load<User>("users.json");
        Gyms = _storage.Load<Gym>("gyms.json");
        Plans = _storage.Load<SubscriptionPlan>("plans.json");
        Trainers = _storage.Load<Trainer>("trainers.json");
        Classes = _storage.Load<FitnessClass>("classes.json");
        Subscriptions = _storage.Load<ClientSubscription>("subscriptions.json");
        Bookings = _storage.Load<Booking>("bookings.json");
    }

    public void SaveAll()
    {
        _storage.Save("users.json", Users);
        _storage.Save("gyms.json", Gyms);
        _storage.Save("plans.json", Plans);
        _storage.Save("trainers.json", Trainers);
        _storage.Save("classes.json", Classes);
        _storage.Save("subscriptions.json", Subscriptions);
        _storage.Save("bookings.json", Bookings);
    }

    // =========================
    // ADMIN: GYM CRUD
    // =========================
    public Gym CreateGym(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nume sală invalid.");
        var g = new Gym { Name = name.Trim(), Address = (address ?? "").Trim() };
        Gyms.Add(g);
        SaveAll();
        return g;
    }

    public void EditGym(Guid gymId, string newName, string newAddress)
    {
        var g = Gyms.Single(x => x.Id == gymId);
        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Nume invalid.");
        g.Name = newName.Trim();
        g.Address = (newAddress ?? "").Trim();
        SaveAll();
    }

    public void DeleteGym(Guid gymId)
    {
        Gyms.RemoveAll(g => g.Id == gymId);

        var classIds = Classes.Where(c => c.GymId == gymId).Select(c => c.Id).ToHashSet();
        Classes.RemoveAll(c => c.GymId == gymId);
        Bookings.RemoveAll(b => classIds.Contains(b.FitnessClassId));

        SaveAll();
    }

    // =========================
    // ADMIN: ZONE CRUD
    // =========================
    public Zone AddZone(Guid gymId, string zoneName, int cap, TimeOnly open, TimeOnly close)
    {
        var gym = Gyms.Single(g => g.Id == gymId);
        if (string.IsNullOrWhiteSpace(zoneName)) throw new ArgumentException("Nume zonă invalid.");
        if (cap <= 0) throw new ArgumentException("Capacitate invalidă.");
        if (close <= open) throw new ArgumentException("Program invalid.");

        var z = new Zone { Name = zoneName.Trim(), Capacity = cap, OpenTime = open, CloseTime = close };
        gym.Zones.Add(z);
        SaveAll();
        return z;
    }

    public void EditZone(Guid gymId, Guid zoneId, string newName, int newCap, TimeOnly open, TimeOnly close)
    {
        var gym = Gyms.Single(g => g.Id == gymId);
        var z = gym.Zones.Single(x => x.Id == zoneId);

        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Nume zonă invalid.");
        if (newCap <= 0) throw new ArgumentException("Capacitate invalidă.");
        if (close <= open) throw new ArgumentException("Program invalid.");

        z.Name = newName.Trim();
        z.Capacity = newCap;
        z.OpenTime = open;
        z.CloseTime = close;
        SaveAll();
    }

    public void DeleteZone(Guid gymId, Guid zoneId)
    {
        var gym = Gyms.Single(g => g.Id == gymId);

        var classIds = Classes.Where(c => c.GymId == gymId && c.ZoneId == zoneId).Select(c => c.Id).ToHashSet();
        Classes.RemoveAll(c => c.GymId == gymId && c.ZoneId == zoneId);
        Bookings.RemoveAll(b => classIds.Contains(b.FitnessClassId));

        gym.Zones.RemoveAll(z => z.Id == zoneId);
        SaveAll();
    }

    // =========================
    // ADMIN: PLANS CRUD
    // =========================
    public SubscriptionPlan CreatePlan(string name, decimal price, int days, AccessType type)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nume plan invalid.");
        if (price < 0) throw new ArgumentException("Preț invalid.");
        if (days <= 0) throw new ArgumentException("Durată invalidă.");

        var p = new SubscriptionPlan { Name = name.Trim(), Price = price, DurationDays = days, AccessType = type };
        Plans.Add(p);
        SaveAll();
        return p;
    }

    public void EditPlan(Guid planId, string newName, decimal newPrice, int newDays, AccessType newType)
    {
        var p = Plans.Single(x => x.Id == planId);

        if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Nume invalid.");
        if (newPrice < 0) throw new ArgumentException("Preț invalid.");
        if (newDays <= 0) throw new ArgumentException("Durată invalidă.");

        p.Name = newName.Trim();
        p.Price = newPrice;
        p.DurationDays = newDays;
        p.AccessType = newType;
        SaveAll();
    }

    public void DeletePlan(Guid planId)
    {
        Plans.RemoveAll(p => p.Id == planId);
        SaveAll();
    }

    // =========================
    // ADMIN: TRAINERS
    // =========================
    public Trainer CreateTrainer(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Nume antrenor invalid.");
        var t = new Trainer { FullName = fullName.Trim() };
        Trainers.Add(t);
        SaveAll();
        return t;
    }

    // =========================
    // ADMIN: CLASSES CRUD
    // =========================
    public FitnessClass CreateClass(Guid gymId, Guid zoneId, Guid trainerId, string type, DateTime startAt, int durMin, int cap)
    {
        var gym = Gyms.Single(g => g.Id == gymId);
        if (!gym.Zones.Any(z => z.Id == zoneId)) throw new InvalidOperationException("Zona nu există în sala selectată.");
        if (!Trainers.Any(t => t.Id == trainerId)) throw new InvalidOperationException("Antrenor inexistent.");

        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Tip clasă invalid.");
        if (durMin <= 0 || cap <= 0) throw new ArgumentException("Durată/capacitate invalide.");

        var c = new FitnessClass
        {
            GymId = gymId,
            ZoneId = zoneId,
            TrainerId = trainerId,
            Type = type.Trim(),
            StartAt = startAt,
            DurationMin = durMin,
            Capacity = cap
        };

        Classes.Add(c);
        SaveAll();
        return c;
    }

    public void EditClass(Guid classId, string newType, DateTime newStartAt, int newDurMin, int newCap, Guid newTrainerId)
    {
        var c = Classes.Single(x => x.Id == classId);

        if (string.IsNullOrWhiteSpace(newType)) throw new ArgumentException("Tip invalid.");
        if (newDurMin <= 0 || newCap <= 0) throw new ArgumentException("Durată/capacitate invalide.");
        if (!Trainers.Any(t => t.Id == newTrainerId)) throw new InvalidOperationException("Antrenor inexistent.");

        c.Type = newType.Trim();
        c.StartAt = newStartAt;
        c.DurationMin = newDurMin;
        c.Capacity = newCap;
        c.TrainerId = newTrainerId;

        SaveAll();
    }

    public void DeleteClass(Guid classId)
    {
        Classes.RemoveAll(c => c.Id == classId);
        Bookings.RemoveAll(b => b.FitnessClassId == classId);
        SaveAll();
    }

    public List<Booking> GetBookingsForClass(Guid classId)
        => Bookings.Where(b => b.FitnessClassId == classId).ToList();

    public (int ActiveSubs, List<(FitnessClass Class, int Active, int Cap)> Occupancy) GetStats(DateTime now)
    {
        int activeSubs = Subscriptions.Count(s => s.IsActive(now));
        var occ = Classes.OrderBy(c => c.StartAt)
            .Select(c => (c, Bookings.Count(b => b.FitnessClassId == c.Id && b.Status == BookingStatus.Active), c.Capacity))
            .ToList();

        return (activeSubs, occ);
    }

    // =========================
    // CLIENT: BUY SUB + BOOKINGS
    // =========================
    public ClientSubscription BuySubscription(Guid clientId, Guid planId, DateTime now)
    {
        var plan = Plans.Single(p => p.Id == planId);

        var sub = new ClientSubscription
        {
            ClientId = clientId,
            PlanId = planId,
            StartDate = now,
            EndDate = now.AddDays(plan.DurationDays)
        };

        Subscriptions.Add(sub);
        SaveAll();
        return sub;
    }

    public Booking ReserveClass(Guid clientId, Guid classId, DateTime now)
    {
        var cls = Classes.Single(c => c.Id == classId);

        if (cls.StartAt <= now)
            throw new InvalidOperationException("Clasa este în trecut sau începe acum.");

        if (!ClientHasValidSubscriptionForClasses(clientId, now))
            throw new InvalidOperationException("Nu ai abonament valid care include clase.");

        int active = Bookings.Count(b => b.FitnessClassId == classId && b.Status == BookingStatus.Active);
        if (active >= cls.Capacity)
            throw new InvalidOperationException("Clasa este completă.");

        bool already = Bookings.Any(b => b.ClientId == clientId && b.FitnessClassId == classId && b.Status == BookingStatus.Active);
        if (already) throw new InvalidOperationException("Ai deja rezervare activă la această clasă.");

        var booking = new Booking
        {
            ClientId = clientId,
            FitnessClassId = classId,
            CreatedAt = now,
            Status = BookingStatus.Active
        };

        Bookings.Add(booking);
        SaveAll();
        return booking;
    }

    public void CancelBooking(Guid clientId, Guid bookingId, DateTime now, int minHoursBefore = 2)
    {
        var b = Bookings.Single(x => x.Id == bookingId);
        if (b.ClientId != clientId)
            throw new InvalidOperationException("Nu poți anula rezervarea altui client.");

        var cls = Classes.Single(c => c.Id == b.FitnessClassId);
        if (cls.StartAt <= now) throw new InvalidOperationException("Nu mai poți anula: clasa a început.");
        if (cls.StartAt - now < TimeSpan.FromHours(minHoursBefore))
            throw new InvalidOperationException($"Poți anula doar cu cel puțin {minHoursBefore} ore înainte.");

        b.Status = BookingStatus.Cancelled;
        SaveAll();
    }

    public List<ClientSubscription> GetClientSubscriptions(Guid clientId)
        => Subscriptions.Where(s => s.ClientId == clientId).OrderByDescending(s => s.StartDate).ToList();

    public List<Booking> GetClientBookings(Guid clientId)
        => Bookings.Where(b => b.ClientId == clientId).OrderByDescending(b => b.CreatedAt).ToList();

    public int GetActiveBookingsCount(Guid classId)
        => Bookings.Count(b => b.FitnessClassId == classId && b.Status == BookingStatus.Active);

    private bool ClientHasValidSubscriptionForClasses(Guid clientId, DateTime now)
    {
        var activeSubs = Subscriptions.Where(s => s.ClientId == clientId && s.IsActive(now)).ToList();
        if (activeSubs.Count == 0) return false;

        foreach (var sub in activeSubs)
        {
            var plan = Plans.FirstOrDefault(p => p.Id == sub.PlanId);
            if (plan is null) continue;
            if (plan.AccessType == AccessType.Full || plan.AccessType == AccessType.ClassesOnly)
                return true;
        }
        return false;
    }

    private void EnsureAdmin()
    {
        if (Users.Any(u => u.Role == Role.Admin)) return;

        // super simplu pentru varianta ușoară: admin/admin (hash = "admin")
        var admin = new AdminUser
        {
            Username = "admin",
            PasswordHash = "admin"
        };

        Users.Add(admin);
        SaveAll();
    }
}
