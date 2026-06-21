# Highlight Hook

Highlight Hook watches NVIDIA's local `UGCTracker.json` file and tells OBS Studio to save the replay buffer after a user-selected delay.

It keeps the automatic NVIDIA highlight trigger, but saves a longer OBS clip that includes the reaction after the moment.

## How It Works

1. On startup, the app loads the last known highlight ID from NVIDIA's tracker file.
2. It watches `UGCTracker.json` for new highlight entries.
3. When a new valid highlight appears, it waits the configured delay.
4. After the delay, it sends `SaveReplayBuffer` to OBS WebSocket.

## Timing

The graphic below shows the basic flow:

![Highlight Hook timing diagram](assets/timing-diagram.png)

## Batching Nearby Highlights

Highlight Hook can be used to reduce clip spam when several NVIDIA highlights happen close together.

The basic idea is:

1. Read NVIDIA's last highlight ID on startup.
2. Watch for new highlight entries in `UGCTracker.json`.
3. When several new highlights arrive close together, treat them as one burst instead of separate saves.
4. Wait until the burst settles, then save one OBS replay buffer clip for the whole group.

That means one OBS clip can cover multiple NVIDIA highlight entries if they happen inside the same time window.

This is useful when a fight or streak creates several automatic highlight entries in a row. Instead of saving a bunch of overlapping clips, the app can group them and keep the recording list cleaner.

The replay buffer length setting helps with this because it tells the app how much video OBS keeps in memory when deciding whether nearby highlights can be grouped into one useful save.

## Screenshot

Current app screenshot:

![Highlight Hook app screenshot](assets/app-screenshot.png)

## Settings

- `Highlight tracker path`: the NVIDIA `UGCTracker.json` file
- `After-highlight reaction delay`: how long to wait before saving in OBS
- `Replay buffer length`: how much time OBS keeps in memory
- `OBS host`, `OBS port`, `OBS password`: WebSocket connection settings

## Notes

- The app is not limited to Fortnite.
- It works with games that write NVIDIA Highlights into the same local tracker format.
- Replay buffer length is entered manually because OBS does not expose that value over WebSocket.

## Build

```powershell
cd HighlightHook
dotnet build
```

## Run

```powershell
cd HighlightHook
dotnet run
```
