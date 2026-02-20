
using System;
using System.Windows;
using System.Windows.Media;
using MouseHighlighterPro.Models;
using MouseHighlighterPro.Services;

namespace MouseHighlighterPro.Rendering;

public sealed class OverlayRenderer
{
    private OverlayFrame? _frame;

    public void Update(OverlayFrame frame) => _frame = frame;

    public void Render(DrawingContext dc)
    {
        if (_frame is null) return;
        if (!_frame.HasCursor) return;

        var s = _frame.Settings;
        var circle = s.Circle;
        var click = s.ClickEffect;

        var center = _frame.CursorDip;

        double radius = Math.Max(1, circle.Radius);
        double thickness = Math.Max(1, circle.Thickness);

        // Pulse effect
        var strokeColor = circle.Stroke.ToMediaColor();
        if (click.Enabled && _frame.LastClickMs.HasValue && _frame.LastClickButton.HasValue)
        {
            var dt = _frame.NowMs - _frame.LastClickMs.Value;
            if (dt >= 0 && dt <= click.PulseDurationMs)
            {
                var t = dt / (double)click.PulseDurationMs; // 0..1
                // ease-out cubic
                var eased = 1 - Math.Pow(1 - t, 3);
                var scale = 1 + (click.PulseScale - 1) * (1 - eased);
                radius *= scale;
                thickness *= (1 + 0.2 * (1 - eased));

                strokeColor = _frame.LastClickButton.Value switch
                {
                    MouseHookService.MouseButton.Left => click.LeftClickColor.ToMediaColor(),
                    MouseHookService.MouseButton.Right => click.RightClickColor.ToMediaColor(),
                    MouseHookService.MouseButton.Middle => click.MiddleClickColor.ToMediaColor(),
                    _ => strokeColor
                };
            }
        }

        var fillBrush = new SolidColorBrush(circle.Fill.ToMediaColor());
        fillBrush.Freeze();

        var strokeBrush = new SolidColorBrush(strokeColor);
        strokeBrush.Freeze();

        var penMain = new System.Windows.Media.Pen(strokeBrush, thickness) { StartLineCap = PenLineCap.Round, EndLineCap = PenLineCap.Round };
        penMain.Freeze();

        dc.DrawEllipse(fillBrush, penMain, center, radius, radius);
    }
}
