
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace MouseHighlighterPro.Models;

public sealed class AppSettings
{
    public bool OverlayEnabled { get; set; } = true;

    /// <summary>Render target fps (throttle). Actual callback may be tied to monitor refresh.</summary>
    public int TargetFps { get; set; } = 60;

    public CircleStyle Circle { get; set; } = new();
    public ClickEffectSettings ClickEffect { get; set; } = new();
    public HotkeySettings Hotkeys { get; set; } = new();

    public int SchemaVersion { get; set; } = 1;
}

public sealed class CircleStyle
{
    public double Radius { get; set; } = 26;
    public double Thickness { get; set; } = 5;

    public ColorRgba Fill { get; set; } = new(255, 0, 0, 0.20);
    public ColorRgba Stroke { get; set; } = new(255, 0, 0, 0.95);

    public double ShadowBlur { get; set; } = 18;
    public double ShadowOpacity { get; set; } = 0.45;
}

public sealed class ClickEffectSettings
{
    public bool Enabled { get; set; } = true;

    public double PulseScale { get; set; } = 1.35;
    public int PulseDurationMs { get; set; } = 120;

    public ColorRgba LeftClickColor { get; set; } = new(255, 200, 60, 0.95);
    public ColorRgba RightClickColor { get; set; } = new(60, 140, 255, 0.95);
    public ColorRgba MiddleClickColor { get; set; } = new(80, 230, 120, 0.95);
}

public sealed class HotkeySettings
{
    public Hotkey ToggleOverlay { get; set; } = Hotkey.DefaultToggleOverlay;
    public Hotkey OpenSettings { get; set; } = Hotkey.DefaultOpenSettings;
}

public readonly record struct ColorRgba(byte R, byte G, byte B, double A)
{
    public System.Windows.Media.Color ToMediaColor() => System.Windows.Media.Color.FromArgb((byte)(Clamp01(A) * 255), R, G, B);

    private static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
}
