namespace HighlightHook.Utils
{
    public class Debouncer
    {
        private readonly int _delayMilliseconds;
        private readonly object _lock = new();
        private CancellationTokenSource? _cts;

        public Debouncer(int delayMilliseconds)
        {
            _delayMilliseconds = delayMilliseconds;
        }

        public void Debounce(Action action)
        {
            Debounce(() =>
            {
                action();
                return Task.CompletedTask;
            });
        }

        public void Debounce(Func<Task> action)
        {
            CancellationTokenSource? previousCts;
            CancellationTokenSource currentCts;
            lock (_lock)
            {
                previousCts = _cts;
                currentCts = new CancellationTokenSource();
                _cts = currentCts;
            }

            previousCts?.Cancel();
            previousCts?.Dispose();

            var token = currentCts.Token;
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(_delayMilliseconds, token).ConfigureAwait(false);
                    token.ThrowIfCancellationRequested();
                    await action().ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                }
                catch
                {
                    // Swallow debounce exceptions so file watcher callbacks do not crash the app.
                }
                finally
                {
                    lock (_lock)
                    {
                        if (ReferenceEquals(_cts, currentCts))
                        {
                            _cts = null;
                        }
                    }

                    currentCts.Dispose();
                }
            });
        }
    }
}
