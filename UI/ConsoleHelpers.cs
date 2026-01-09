namespace FitnessNetworkApp1.UI;

public static class ConsoleHelpers
{
    public static string ReadNonEmpty(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = (Console.ReadLine() ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(s)) return s;
            Console.WriteLine("Valoare invalidă. Încearcă din nou.");
        }
    }

    public static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (int.TryParse(Console.ReadLine(), out var v)) return v;
            Console.WriteLine("Număr invalid.");
        }
    }

    public static decimal ReadDecimal(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (decimal.TryParse(Console.ReadLine(), out var v)) return v;
            Console.WriteLine("Valoare invalidă.");
        }
    }

    public static Guid ReadGuid(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (Guid.TryParse(Console.ReadLine(), out var g)) return g;
            Console.WriteLine("Guid invalid. Copiază exact ID-ul afișat.");
        }
    }

    public static DateTime ReadDateTime(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (DateTime.TryParse(Console.ReadLine(), out var dt)) return dt;
            Console.WriteLine("Dată/ora invalidă. Exemplu: 2026-01-09 18:30");
        }
    }

    public static TimeOnly ReadTimeOnly(string label)
    {
        while (true)
        {
            Console.Write(label);
            if (TimeOnly.TryParse(Console.ReadLine(), out var t)) return t;
            Console.WriteLine("Ora invalidă. Exemplu: 08:00");
        }
    }
}