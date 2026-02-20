
using System;
using System.Windows.Media;
using MouseHighlighterPro.Models;
using MouseHighlighterPro.Utils;

namespace MouseHighlighterPro.ViewModels;

public sealed class SettingsViewModel : ObservableObject
{
    private readonly AppSettings _settings;

    public SettingsViewModel(AppSettings settings)
    {
        _settings = settings;

        SaveCommand = new RelayCommand(() => SaveRequested?.Invoke(this, EventArgs.Empty));
        CancelCommand = new RelayCommand(() => CancelRequested?.Invoke(this, EventArgs.Empty));
    }

    public event EventHandler? SaveRequested;
    public event EventHandler? CancelRequested;

    public RelayCommand SaveCommand { get; }
    public RelayCommand CancelCommand { get; }

    /// <summary>Mutable settings instance (used for preview rendering).</summary>
    public AppSettings Settings => _settings;

    public bool OverlayEnabled
    {
        get => _settings.OverlayEnabled;
        set { _settings.OverlayEnabled = value; Notify(); }
    }

    public int TargetFps
    {
        get => _settings.TargetFps;
        set { _settings.TargetFps = Math.Clamp(value, 15, 240); Notify(); }
    }

    public double Radius
    {
        get => _settings.Circle.Radius;
        set { _settings.Circle.Radius = Math.Clamp(value, 1, 300); Notify(); Notify(nameof(PreviewBrush)); Notify(nameof(PreviewStroke)); }
    }

    public double Thickness
    {
        get => _settings.Circle.Thickness;
        set { _settings.Circle.Thickness = Math.Clamp(value, 1, 60); Notify(); }
    }

    public double FillA
    {
        get => _settings.Circle.Fill.A;
        set { _settings.Circle.Fill = _settings.Circle.Fill with { A = Math.Clamp(value, 0, 1) }; Notify(); Notify(nameof(PreviewBrush)); }
    }

    public byte FillR
    {
        get => _settings.Circle.Fill.R;
        set { _settings.Circle.Fill = _settings.Circle.Fill with { R = value }; Notify(); Notify(nameof(PreviewBrush)); }
    }

    public byte FillG
    {
        get => _settings.Circle.Fill.G;
        set { _settings.Circle.Fill = _settings.Circle.Fill with { G = value }; Notify(); Notify(nameof(PreviewBrush)); }
    }

    public byte FillB
    {
        get => _settings.Circle.Fill.B;
        set { _settings.Circle.Fill = _settings.Circle.Fill with { B = value }; Notify(); Notify(nameof(PreviewBrush)); }
    }

    public double StrokeA
    {
        get => _settings.Circle.Stroke.A;
        set { _settings.Circle.Stroke = _settings.Circle.Stroke with { A = Math.Clamp(value, 0, 1) }; Notify(); Notify(nameof(PreviewStroke)); }
    }

    public byte StrokeR
    {
        get => _settings.Circle.Stroke.R;
        set { _settings.Circle.Stroke = _settings.Circle.Stroke with { R = value }; Notify(); Notify(nameof(PreviewStroke)); }
    }

    public byte StrokeG
    {
        get => _settings.Circle.Stroke.G;
        set { _settings.Circle.Stroke = _settings.Circle.Stroke with { G = value }; Notify(); Notify(nameof(PreviewStroke)); }
    }

    public byte StrokeB
    {
        get => _settings.Circle.Stroke.B;
        set { _settings.Circle.Stroke = _settings.Circle.Stroke with { B = value }; Notify(); Notify(nameof(PreviewStroke)); }
    }

    public double ShadowBlur
    {
        get => _settings.Circle.ShadowBlur;
        set { _settings.Circle.ShadowBlur = Math.Clamp(value, 0, 100); Notify(); }
    }

    public double ShadowOpacity
    {
        get => _settings.Circle.ShadowOpacity;
        set { _settings.Circle.ShadowOpacity = Math.Clamp(value, 0, 1); Notify(); }
    }

    public bool ClickPulseEnabled
    {
        get => _settings.ClickEffect.Enabled;
        set { _settings.ClickEffect.Enabled = value; Notify(); }
    }

    public double PulseScale
    {
        get => _settings.ClickEffect.PulseScale;
        set { _settings.ClickEffect.PulseScale = Math.Clamp(value, 1.0, 3.0); Notify(); }
    }

    public int PulseDurationMs
    {
        get => _settings.ClickEffect.PulseDurationMs;
        set { _settings.ClickEffect.PulseDurationMs = Math.Clamp(value, 30, 800); Notify(); }
    }

    // Fallback preview brushes (still useful for small swatches if needed)
    public System.Windows.Media.Brush PreviewBrush => new SolidColorBrush(_settings.Circle.Fill.ToMediaColor());
    public System.Windows.Media.Brush PreviewStroke => new SolidColorBrush(_settings.Circle.Stroke.ToMediaColor());
}
