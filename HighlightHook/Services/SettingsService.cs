using HighlightHook.Models;
using System.Text.Json;

namespace HighlightHook.Services
{
    public static class SettingsService
    {
        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HighlightHook");
        private static readonly string SettingsFile = Path.Combine(AppFolder, "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (!Directory.Exists(AppFolder))
                {
                    Directory.CreateDirectory(AppFolder);
                }

                if (!File.Exists(SettingsFile))
                {
                    return new AppSettings();
                }

                var json = File.ReadAllText(SettingsFile);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var settings = JsonSerializer.Deserialize<AppSettings>(json, options);
                return settings ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public static void Save(AppSettings settings)
        {
            try
            {
                if (!Directory.Exists(AppFolder))
                {
                    Directory.CreateDirectory(AppFolder);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(SettingsFile, json);
            }
            catch
            {
                // Ignore save failures for now.
            }
        }
    }
}
