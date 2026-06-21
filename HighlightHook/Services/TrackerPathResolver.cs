namespace HighlightHook.Services
{
    public static class TrackerPathResolver
    {
        public static string GetDefaultTrackerPath()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!string.IsNullOrWhiteSpace(localAppData))
            {
                return Path.Combine(localAppData, "NVIDIA Corporation", "NVIDIA Overlay", "UGCTracker.json");
            }

            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userProfile, "AppData", "Local", "NVIDIA Corporation", "NVIDIA Overlay", "UGCTracker.json");
        }

        public static bool TryGetDefaultTrackerPath(out string path)
        {
            path = GetDefaultTrackerPath();
            return File.Exists(path);
        }
    }
}
