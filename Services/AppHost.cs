
using System.Windows;
using MouseHighlighterPro.Models;
using MouseHighlighterPro.ViewModels;
using MouseHighlighterPro.Windows;

namespace MouseHighlighterPro.Services;

public sealed class AppHost : IDisposable
{
    private readonly SettingsRepository _repo = new();
    private readonly TrayService _tray = new();
    private readonly HotkeyService _hotkeys = new();
    private readonly MouseHookService _mouseHook = new();

    private AppSettings _settings = new();
    private OverlayWindow? _overlay;
    private int _hkToggleId;
    private int _hkSettingsId;

    public void Start()
    {
        _settings = _repo.LoadOrDefault();

        _overlay = new OverlayWindow(_settings);
        _overlay.Show();

        _tray.ToggleOverlayRequested += (_, _) => ToggleOverlay();
        _tray.OpenSettingsRequested += (_, _) => OpenSettings();
        _tray.ExitRequested += (_, _) => Exit();

        _tray.SetOverlayEnabled(_settings.OverlayEnabled);

        _mouseHook.Clicked += (_, e) =>
        {
            _overlay?.SetLastClick(e.TimestampMs, e.Button);
        };
        _mouseHook.Start();

        _hotkeys.HotkeyPressed += (_, id) =>
        {
            if (id == _hkToggleId) ToggleOverlay();
            else if (id == _hkSettingsId) OpenSettings();
        };

        TryRegisterHotkeys();
    }

    private void TryRegisterHotkeys()
    {
        try
        {
            _hotkeys.Initialize();
            _hotkeys.UnregisterAll();

            _hkToggleId = _hotkeys.Register(_settings.Hotkeys.ToggleOverlay, "ToggleOverlay");
            _hkSettingsId = _hotkeys.Register(_settings.Hotkeys.OpenSettings, "OpenSettings");
        }
        catch
        {
            // If hotkey registration fails, keep app working without hotkeys.
            _hkToggleId = 0;
            _hkSettingsId = 0;
        }
    }

    private void ToggleOverlay()
    {
        _settings.OverlayEnabled = !_settings.OverlayEnabled;
        _tray.SetOverlayEnabled(_settings.OverlayEnabled);

        _overlay?.ApplySettings(_settings);
        _repo.Save(_settings);
    }

    private void OpenSettings()
    {
        // Clone settings to allow cancel without side effects
        var temp = DeepClone(_settings);

        var vm = new SettingsViewModel(temp);
        var win = new SettingsWindow(vm);
        var result = win.ShowDialog();

        if (result == true)
        {
            _settings = temp;
            _overlay?.ApplySettings(_settings);
            _tray.SetOverlayEnabled(_settings.OverlayEnabled);
            _repo.Save(_settings);
            TryRegisterHotkeys();
        }
    }

    private void Exit()
    {
        System.Windows.Application.Current.Shutdown();
    }

    public void Dispose()
    {
        _mouseHook.Dispose();
        _hotkeys.Dispose();
        _tray.Dispose();
        if (_overlay != null)
        {
            _overlay.Close();
            _overlay = null;
        }
    }

    private static AppSettings DeepClone(AppSettings s)
    {
        // Manual clone (keeps it fast and dependency-free)
        return new AppSettings
        {
            OverlayEnabled = s.OverlayEnabled,
            TargetFps = s.TargetFps,
            SchemaVersion = s.SchemaVersion,
            Circle = new CircleStyle
            {
                Radius = s.Circle.Radius,
                Thickness = s.Circle.Thickness,
                Fill = s.Circle.Fill,
                Stroke = s.Circle.Stroke,
                ShadowBlur = s.Circle.ShadowBlur,
                ShadowOpacity = s.Circle.ShadowOpacity,
            },
            ClickEffect = new ClickEffectSettings
            {
                Enabled = s.ClickEffect.Enabled,
                PulseScale = s.ClickEffect.PulseScale,
                PulseDurationMs = s.ClickEffect.PulseDurationMs,
                LeftClickColor = s.ClickEffect.LeftClickColor,
                RightClickColor = s.ClickEffect.RightClickColor,
                MiddleClickColor = s.ClickEffect.MiddleClickColor,
            },
            Hotkeys = new HotkeySettings
            {
                ToggleOverlay = s.Hotkeys.ToggleOverlay,
                OpenSettings = s.Hotkeys.OpenSettings,
            }
        };
    }
}
