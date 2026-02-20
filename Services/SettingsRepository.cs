
using System;
using System.IO;
using System.Text.Json;
using MouseHighlighterPro.Models;

namespace MouseHighlighterPro.Services;

public sealed class SettingsRepository
{
    private readonly string _settingsPath;

    public SettingsRepository(string appName = "MouseHighlighterPro")
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            appName);

        Directory.CreateDirectory(dir);
        _settingsPath = Path.Combine(dir, "settings.json");
    }

    public AppSettings LoadOrDefault()
    {
        try
        {
            if (!File.Exists(_settingsPath))
                return new AppSettings();

            var json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions()) ?? new AppSettings();

            // Basic migration point
            if (settings.SchemaVersion < 1)
                settings.SchemaVersion = 1;

            return settings;
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, JsonOptions(pretty: true));
        File.WriteAllText(_settingsPath, json);
    }

    private static JsonSerializerOptions JsonOptions(bool pretty = false) => new()
    {
        WriteIndented = pretty,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };
}
