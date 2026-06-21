using HighlightHook.Models;
using System.Text.Json;

namespace HighlightHook.Services
{
    public static class UgcTrackerReader
    {
        public static async Task<UgcTracker?> ReadTrackerAsync(string path, CancellationToken cancellationToken = default)
        {
            var attempts = 0;
            while (attempts < 10)
            {
                attempts++;
                try
                {
                    var json = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true
                    };
                    var tracker = JsonSerializer.Deserialize<UgcTracker>(json, options);
                    return tracker;
                }
                catch (IOException)
                {
                    await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                }
                catch (UnauthorizedAccessException)
                {
                    await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                }
                catch (JsonException)
                {
                    await Task.Delay(250, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
            }

            return null;
        }
    }
}
