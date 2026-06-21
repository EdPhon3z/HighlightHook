namespace HighlightHook.Services
{
    public static class LogService
    {
        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HighlightHook");
        private static readonly string LogFile = Path.Combine(AppFolder, "HighlightHook.log");
        private static readonly string OldLogFile = Path.Combine(AppFolder, "HighlightHook.old.log");
        private const long MaxLogSize = 1_000_000;
        private static readonly object LockObj = new();

        public static void Write(string message)
        {
            try
            {
                lock (LockObj)
                {
                    if (!Directory.Exists(AppFolder))
                    {
                        Directory.CreateDirectory(AppFolder);
                    }

                    RotateIfNeeded();
                    var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}" + Environment.NewLine;
                    File.AppendAllText(LogFile, line);
                }
            }
            catch
            {
                // ignore logging errors
            }
        }

        private static void RotateIfNeeded()
        {
            try
            {
                if (File.Exists(LogFile))
                {
                    var length = new FileInfo(LogFile).Length;
                    if (length > MaxLogSize)
                    {
                        if (File.Exists(OldLogFile))
                        {
                            File.Delete(OldLogFile);
                        }
                        File.Move(LogFile, OldLogFile);
                    }
                }
            }
            catch
            {
                // ignore rotation errors
            }
        }
    }
}
