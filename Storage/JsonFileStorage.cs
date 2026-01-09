using System.Text.Json;

namespace FitnessNetworkApp1.Storage;

public class JsonFileStorage
{
    private readonly string _dataFolder;
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public JsonFileStorage(string dataFolder)
    {
        _dataFolder = dataFolder;
        Directory.CreateDirectory(_dataFolder);
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(_dataFolder, fileName);
    }

    public List<T> Load<T>(string fileName)
    {
        var path = GetPath(fileName);

        if (!File.Exists(path))
            return new List<T>();

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<List<T>>(json, _options);
            return data ?? new List<T>();
        }
        catch
        {
            // fișier lipsă sau corupt → listă goală
            return new List<T>();
        }
    }

    public void Save<T>(string fileName, List<T> data)
    {
        var path = GetPath(fileName);
        var json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(path, json);
    }
}