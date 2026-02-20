
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MouseHighlighterPro.Models;
using MouseHighlighterPro.Rendering;
using MouseHighlighterPro.Services;

namespace MouseHighlighterPro.Windows;

public partial class OverlayWindow : Window
{
    private readonly CursorTracker _cursorTracker = new();
    private readonly RenderLoop _renderLoop;

    private AppSettings _settings;
    private long? _lastClickMs;
    private MouseHookService.MouseButton? _lastClickButton;

    public OverlayWindow(AppSettings settings)
    {
        InitializeComponent();

        _settings = settings;

        // Cover virtual screen (multi-monitor)
        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;

        WindowStartupLocation = WindowStartupLocation.Manual;

        _renderLoop = new RenderLoop(Tick, settings.TargetFps);
        Loaded += (_, _) => _renderLoop.Start();
        Unloaded += (_, _) => _renderLoop.Stop();
    }

    public void ApplySettings(AppSettings settings)
    {
        _settings = settings;
        _renderLoop.SetTargetFps(settings.TargetFps);
    }

    public void SetLastClick(long timestampMs, MouseHookService.MouseButton button)
    {
        _lastClickMs = timestampMs;
        _lastClickButton = button;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        var hwnd = new WindowInteropHelper(this).Handle;

        // Click-through + layered + toolwindow (hide from Alt+Tab)
        var ex = GetWindowLongPtr(hwnd, GWL_EXSTYLE).ToInt64();
        ex |= WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOOLWINDOW;
        SetWindowLongPtr(hwnd, GWL_EXSTYLE, new IntPtr(ex));

        // Ensure topmost (some apps may fight it)
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);
    }

    private void Tick()
    {
        if (!_settings.OverlayEnabled)
        {
            if (Visibility == Visibility.Visible)
                Visibility = Visibility.Hidden;
            return;
        }

        if (Visibility != Visibility.Visible)
            Visibility = Visibility.Visible;

        var source = PresentationSource.FromVisual(this);
        _cursorTracker.TryUpdate(source);

        Surface.Update(new OverlayFrame
        {
            CursorDip = _cursorTracker.CurrentDip,
            HasCursor = _cursorTracker.HasValidPosition,
            Settings = _settings,
            NowMs = Environment.TickCount64,
            LastClickMs = _lastClickMs,
            LastClickButton = _lastClickButton
        });
    }

    #region Win32
    private const int GWL_EXSTYLE = -20;

    private const long WS_EX_TRANSPARENT = 0x20L;
    private const long WS_EX_LAYERED = 0x80000L;
    private const long WS_EX_TOOLWINDOW = 0x80L;

    private static readonly IntPtr HWND_TOPMOST = new(-1);

    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOACTIVATE = 0x0010;
    private const uint SWP_SHOWWINDOW = 0x0040;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    #endregion
}
