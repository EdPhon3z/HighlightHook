using System.Text.Json.Serialization;

namespace HighlightHook.Models
{
    public class UgcTracker
    {
        [JsonPropertyName("UGC")]
        public List<UgcEntry>? UGC { get; set; }

        [JsonPropertyName("info")]
        public TrackerInfo? Info { get; set; }
    }

    public class UgcEntry
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("subType")]
        public string? SubType { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("profileName")]
        public string? ProfileName { get; set; }

        [JsonPropertyName("highlightDefinitionId")]
        public string? HighlightDefinitionId { get; set; }

        [JsonPropertyName("highlightName")]
        public string? HighlightName { get; set; }

        [JsonPropertyName("highlightEventTimeStamp")]
        public DateTime? HighlightEventTimeStamp { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("msDuration")]
        public long? MsDuration { get; set; }
    }

    public class TrackerInfo
    {
        [JsonPropertyName("lastUGCIdUsed")]
        public long LastUGCIdUsed { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("folderCRC")]
        public uint FolderCRC { get; set; }
    }

    public class AppSettings
    {
        public string TrackerPath { get; set; } = string.Empty;
        public string ObsHost { get; set; } = "localhost";
        public int ObsPort { get; set; } = 4455;
        public string ObsPassword { get; set; } = string.Empty;
        public int DelaySeconds { get; set; } = 45;
        public int ReplayBufferSeconds { get; set; } = 60;
        public string EventFilterMode { get; set; } = "All";
        public bool StartMinimized { get; set; } = false;
    }
}
