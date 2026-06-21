using HighlightHook.Models;
using HighlightHook.Services;
using System.Net.Sockets;

namespace HighlightHook;

public partial class Form1 : Form
{
    private AppSettings _settings = new();
    private HighlightWatcher? _watcher;
    private CancellationTokenSource? _saveCts;
    private ObsWebSocketClient? _obsClient;
    private DateTime _lastSaveTime = DateTime.MinValue;
    private readonly TimeSpan _saveCooldown = TimeSpan.FromSeconds(10);

    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        _settings = SettingsService.Load();
        txtObsHost.Text = _settings.ObsHost;
        numObsPort.Value = _settings.ObsPort;
        txtObsPassword.Text = _settings.ObsPassword;
        trackBarDelay.Value = Math.Clamp(_settings.DelaySeconds, 10, 120);
        numReplayBufferSeconds.Value = Math.Clamp(_settings.ReplayBufferSeconds, 10, 600);
        UpdateDelayLabel();
        UpdateReplayBufferLabel();

        if (!string.IsNullOrWhiteSpace(_settings.TrackerPath))
        {
            txtTrackerPath.Text = _settings.TrackerPath;
        }
        else if (TrackerPathResolver.TryGetDefaultTrackerPath(out var defaultPath))
        {
            txtTrackerPath.Text = defaultPath;
        }
        else
        {
            txtTrackerPath.Text = TrackerPathResolver.GetDefaultTrackerPath();
        }

        SaveSettings();
        await InitializeTrackerAsync();
    }

    private async Task<bool> InitializeTrackerAsync()
    {
        var trackerPath = txtTrackerPath.Text.Trim();
        _watcher?.Dispose();
        _watcher = null;

        if (string.IsNullOrWhiteSpace(trackerPath))
        {
            SetStatus("Tracker path not set.");
            return false;
        }

        if (!File.Exists(trackerPath))
        {
            SetStatus("Tracker not found. Browse to UGCTracker.json.");
            LogService.Write("Tracker path not found: " + trackerPath);
            return false;
        }

        try
        {
            _watcher = new HighlightWatcher(trackerPath);
            _watcher.HighlightDetected += Watcher_HighlightDetected;
            _watcher.StatusUpdated += Watcher_StatusUpdated;

            if (await _watcher.InitializeBaselineAsync())
            {
                SetStatus($"Tracker found. Current last Highlight ID: {_watcher.LastSeenId}");
                LogService.Write("Tracker found and baseline initialized.");
                return true;
            }
            else
            {
                SetStatus("Tracker found but could not initialize baseline.");
                LogService.Write("Tracker found but baseline initialization failed.");
                _watcher.Dispose();
                _watcher = null;
                return false;
            }
        }
        catch (Exception ex)
        {
            SetStatus("Could not read tracker file.");
            LogService.Write("InitializeTrackerAsync failed: " + ex.Message);
            _watcher?.Dispose();
            _watcher = null;
            return false;
        }
    }

    private void UpdateDelayLabel()
    {
        lblDelayValue.Text = $"{trackBarDelay.Value} seconds";
    }

    private void UpdateReplayBufferLabel()
    {
        lblReplayBufferValue.Text = $"{numReplayBufferSeconds.Value} seconds";
    }

    private void trackBarDelay_ValueChanged(object sender, EventArgs e)
    {
        UpdateDelayLabel();
        _settings.DelaySeconds = trackBarDelay.Value;
        SaveSettings();
    }

    private void numReplayBufferSeconds_ValueChanged(object sender, EventArgs e)
    {
        UpdateReplayBufferLabel();
        _settings.ReplayBufferSeconds = (int)numReplayBufferSeconds.Value;
        SaveSettings();
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog();
        dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        dialog.Title = "Select UGCTracker.json";
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtTrackerPath.Text = dialog.FileName;
            _settings.TrackerPath = dialog.FileName;
            SaveSettings();
            _ = InitializeTrackerAsync();
        }
    }

    private async void btnStart_Click(object sender, EventArgs e)
    {
        btnStart.Enabled = false;
        btnStop.Enabled = true;
        _settings.ObsHost = txtObsHost.Text.Trim();
        _settings.ObsPort = (int)numObsPort.Value;
        _settings.ObsPassword = txtObsPassword.Text;
        _settings.DelaySeconds = trackBarDelay.Value;
        _settings.TrackerPath = txtTrackerPath.Text.Trim();
        SaveSettings();

        if (!File.Exists(_settings.TrackerPath))
        {
            SetStatus("Highlight tracker file not found. Browse to UGCTracker.json.");
            LogService.Write("Start failed: tracker file missing.");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            return;
        }

        if (string.IsNullOrWhiteSpace(_settings.ObsHost))
        {
            SetStatus("OBS host is required.");
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            return;
        }

        _obsClient?.Dispose();
        _obsClient = new ObsWebSocketClient(_settings.ObsHost, _settings.ObsPort);
        try
        {
            SetStatus("Connecting to OBS...");
            LogService.Write("Attempting OBS connection.");
            await _obsClient.ConnectAsync(_settings.ObsPassword);
            SetStatus("Connected to OBS.");
            LogService.Write("Connected to OBS.");
        }
        catch (Exception ex)
        {
            SetStatus("Could not connect to OBS WebSocket. Check OBS is open and WebSocket is enabled.");
            LogService.Write("OBS connection failed: " + ex.Message);
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            return;
        }

        if (_watcher == null)
        {
            _watcher = new HighlightWatcher(_settings.TrackerPath);
            _watcher.HighlightDetected += Watcher_HighlightDetected;
            _watcher.StatusUpdated += Watcher_StatusUpdated;
        }

        if (await InitializeTrackerAsync())
        {
            _watcher?.Start();
            return;
        }

        btnStart.Enabled = true;
        btnStop.Enabled = false;
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        _watcher?.Stop();
        _watcher?.Dispose();
        _watcher = null;
        _obsClient?.Dispose();
        _obsClient = null;
        _saveCts?.Cancel();
        _saveCts?.Dispose();
        _saveCts = null;
        btnStart.Enabled = true;
        btnStop.Enabled = false;
        SetStatus("Stopped.");
        LogService.Write("Watching stopped.");
    }

    private async void Watcher_HighlightDetected(object? sender, UgcEntry e)
    {
        var now = DateTime.UtcNow;
        if (now - _lastSaveTime < _saveCooldown)
        {
            LogService.Write($"Skipped duplicate or cooldown on highlight id {e.Id}.");
            return;
        }

        _lastSaveTime = now;
        SetStatus($"Highlight detected: {e.ProfileName} - {e.HighlightName}. Saving OBS replay in {_settings.DelaySeconds}s.");
        LogService.Write($"Highlight detected: id={e.Id}, profile={e.ProfileName}, name={e.HighlightName}");

        _saveCts?.Cancel();
        _saveCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.DelaySeconds), _saveCts.Token);
            await SaveReplayAsync(_saveCts.Token);
        }
        catch (TaskCanceledException)
        {
            LogService.Write("Save delay canceled.");
        }
    }

    private async Task SaveReplayAsync(CancellationToken cancellationToken)
    {
        if (_obsClient == null)
        {
            SetStatus("OBS client not initialized.");
            return;
        }

        try
        {
            await _obsClient.SaveReplayBufferAsync(cancellationToken);
            SetStatus("OBS replay buffer saved.");
            LogService.Write("OBS SaveReplayBuffer succeeded.");
        }
        catch (InvalidOperationException ex)
        {
            SetStatus("OBS failed to save replay buffer.");
            LogService.Write("OBS save failed: " + ex.Message);
        }
        catch (Exception ex)
        {
            SetStatus("OBS save failed.");
            LogService.Write("OBS save error: " + ex.Message);
        }
    }

    private void Watcher_StatusUpdated(object? sender, string status)
    {
        SetStatus(status);
    }

    private void SetStatus(string text)
    {
        if (IsDisposed)
        {
            return;
        }

        if (InvokeRequired && IsHandleCreated)
        {
            Invoke(() => lblStatus.Text = text);
            return;
        }

        lblStatus.Text = text;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        _watcher?.Dispose();
        _watcher = null;
        _obsClient?.Dispose();
        _obsClient = null;
        _saveCts?.Cancel();
        _saveCts?.Dispose();
        _saveCts = null;
        SaveSettings();
    }

    private void SaveSettings()
    {
        try
        {
            _settings.TrackerPath = txtTrackerPath.Text.Trim();
            _settings.ObsHost = txtObsHost.Text.Trim();
            _settings.ObsPort = (int)numObsPort.Value;
            _settings.ObsPassword = txtObsPassword.Text;
            _settings.DelaySeconds = trackBarDelay.Value;
            _settings.ReplayBufferSeconds = (int)numReplayBufferSeconds.Value;
            SettingsService.Save(_settings);
        }
        catch
        {
            // ignore
        }
    }

    private void txtTrackerPath_TextChanged(object sender, EventArgs e)
    {
        _settings.TrackerPath = txtTrackerPath.Text.Trim();
        SaveSettings();
    }

    private void helpHowItWorksToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "Highlight Hook watches NVIDIA's UGCTracker.json for new Highlight entries.\n\n" +
            "When a new highlight appears, the app waits the after-highlight delay, then tells OBS to save the replay buffer.\n\n" +
            "Replay Buffer Length is the size of the clip OBS keeps in memory. Because OBS does not expose that value over WebSocket, you enter it here so future batching logic can group nearby highlights into one save.",
            "How It Works",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void helpChangeLogToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "Recent changes:\n" +
            "- Added replay buffer length setting\n" +
            "- Added Help menu and on-screen explanation\n" +
            "- Prepared the app for batching nearby highlights into fewer OBS saves",
            "Change Log",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
