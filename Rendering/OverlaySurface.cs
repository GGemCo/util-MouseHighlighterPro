
using System;
using System.Windows;
using System.Windows.Media;
using MouseHighlighterPro.Models;

namespace MouseHighlighterPro.Rendering;

public sealed class OverlaySurface : FrameworkElement
{
    private readonly OverlayRenderer _renderer = new();

    public OverlaySurface()
    {
        IsHitTestVisible = false;
        SnapsToDevicePixels = false;
    }

    public void Update(OverlayFrame frame)
    {
        _renderer.Update(frame);
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext dc)
    {
        dc.DrawRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(0, 0, ActualWidth, ActualHeight));
        _renderer.Render(dc);
    }
}

public sealed class OverlayFrame
{
    public required System.Windows.Point CursorDip { get; init; }
    public required bool HasCursor { get; init; }
    public required AppSettings Settings { get; init; }
    public required long NowMs { get; init; }

    public long? LastClickMs { get; init; }
    public MouseHighlighterPro.Services.MouseHookService.MouseButton? LastClickButton { get; init; }
}
