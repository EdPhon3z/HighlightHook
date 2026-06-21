using HighlightHook.Models;
using HighlightHook.Utils;

namespace HighlightHook.Services
{
    public class HighlightWatcher : IDisposable
    {
        private const int DebounceMilliseconds = 800;
        private readonly Debouncer _debouncer;
        private readonly object _sync = new();
        private readonly Dictionary<long, DateTime> _recentTriggeredIds = new();
        private FileSystemWatcher? _watcher;
        private System.Threading.Timer? _cleanupTimer;

        public event EventHandler<UgcEntry>? HighlightDetected;
        public event EventHandler<string>? StatusUpdated;

        public long LastSeenId { get; private set; }
        public string TrackerPath { get; }
        public bool IsWatching { get; private set; }

        public HighlightWatcher(string trackerPath)
        {
            TrackerPath = trackerPath;
            _debouncer = new Debouncer(DebounceMilliseconds);
        }

        public async Task<bool> InitializeBaselineAsync(CancellationToken cancellationToken = default)
        {
            if (!File.Exists(TrackerPath))
            {
                StatusUpdated?.Invoke(this, "Tracker file not found.");
                return false;
            }

            var tracker = await UgcTrackerReader.ReadTrackerAsync(TrackerPath, cancellationToken).ConfigureAwait(false);
            if (tracker?.Info == null)
            {
                StatusUpdated?.Invoke(this, "Tracker file is invalid or empty.");
                return false;
            }

            LastSeenId = tracker.Info.LastUGCIdUsed;
            StatusUpdated?.Invoke(this, $"Tracker found. Current last Highlight ID: {LastSeenId}");
            StartCleanupTimer();
            return true;
        }

        public void Start()
        {
            if (IsWatching)
            {
                return;
            }

            var directory = Path.GetDirectoryName(TrackerPath);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                StatusUpdated?.Invoke(this, "Tracker directory not found.");
                return;
            }

            _watcher = new FileSystemWatcher(directory, Path.GetFileName(TrackerPath))
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                IncludeSubdirectories = false
            };

            _watcher.Changed += OnFileChanged;
            _watcher.Created += OnFileChanged;
            _watcher.Renamed += OnFileChanged;
            _watcher.EnableRaisingEvents = true;

            IsWatching = true;
            StatusUpdated?.Invoke(this, "Watching for Highlights.");
        }

        public void Stop()
        {
            if (!IsWatching)
            {
                return;
            }

            _watcher?.Dispose();
            _watcher = null;
            IsWatching = false;
            StatusUpdated?.Invoke(this, "Stopped watching.");
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            _debouncer.Debounce(ProcessFileAsync);
        }

        private async Task ProcessFileAsync()
        {
            try
            {
                var tracker = await UgcTrackerReader.ReadTrackerAsync(TrackerPath).ConfigureAwait(false);
                if (tracker?.UGC == null)
                {
                    return;
                }

                var newEntries = tracker.UGC
                    .Where(entry => entry.Id > LastSeenId)
                    .OrderBy(entry => entry.Id)
                    .ToList();

                if (!newEntries.Any())
                {
                    return;
                }

                var highestId = LastSeenId;
                foreach (var entry in newEntries)
                {
                    if (entry.Id <= LastSeenId)
                    {
                        continue;
                    }

                    if (!IsValidHighlight(entry))
                    {
                        highestId = Math.Max(highestId, entry.Id);
                        continue;
                    }

                    lock (_sync)
                    {
                        var now = DateTime.UtcNow;
                        CleanupRecent(now);
                        if (_recentTriggeredIds.ContainsKey(entry.Id))
                        {
                            highestId = Math.Max(highestId, entry.Id);
                            continue;
                        }

                        _recentTriggeredIds[entry.Id] = now;
                    }

                    HighlightDetected?.Invoke(this, entry);
                    highestId = Math.Max(highestId, entry.Id);
                }

                if (highestId > LastSeenId)
                {
                    LastSeenId = highestId;
                }
            }
            catch (Exception)
            {
                // ignore errors here; log at higher level if needed
            }
        }

        private bool IsValidHighlight(UgcEntry entry)
        {
            return string.Equals(entry.SubType, "Highlight", StringComparison.OrdinalIgnoreCase)
                && string.Equals(entry.Source, "GFE_SDK", StringComparison.OrdinalIgnoreCase);
        }

        private void StartCleanupTimer()
        {
            _cleanupTimer ??= new System.Threading.Timer(_ => CleanupRecent(DateTime.UtcNow), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        private void CleanupRecent(DateTime now)
        {
            lock (_sync)
            {
                var cutoff = now.AddMinutes(-5);
                var expiredKeys = _recentTriggeredIds.Where(kvp => kvp.Value < cutoff).Select(kvp => kvp.Key).ToList();
                foreach (var key in expiredKeys)
                {
                    _recentTriggeredIds.Remove(key);
                }
            }
        }

        public void Dispose()
        {
            _watcher?.Dispose();
            _cleanupTimer?.Dispose();
        }
    }
}
