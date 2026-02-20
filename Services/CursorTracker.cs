
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace MouseHighlighterPro.Services;

public sealed class CursorTracker
{
    public System.Windows.Point CurrentDip { get; private set; }
    public bool HasValidPosition { get; private set; }

    /// <summary>
    /// Updates CurrentDip using current cursor position.
    /// Requires a valid PresentationSource (for DPI conversion).
    /// </summary>
    public bool TryUpdate(PresentationSource? source)
    {
        if (!GetCursorPos(out POINT pt))
        {
            HasValidPosition = false;
            return false;
        }

        var dip = ToDip(source, pt.X, pt.Y);
        HasValidPosition = true;
        CurrentDip = dip;
        return true;
    }

    private static System.Windows.Point ToDip(PresentationSource? source, int xPx, int yPx)
    {
        if (source?.CompositionTarget is null)
            return new System.Windows.Point(xPx, yPx);

        var m = source.CompositionTarget.TransformFromDevice;
        return m.Transform(new System.Windows.Point(xPx, yPx));
    }

    #region Win32
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);
    #endregion
}
