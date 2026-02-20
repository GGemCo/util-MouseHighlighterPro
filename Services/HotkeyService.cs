
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using MouseHighlighterPro.Models;

namespace MouseHighlighterPro.Services;

public sealed class HotkeyService : IDisposable
{
    public event EventHandler<int>? HotkeyPressed;

    private readonly Dictionary<int, (Hotkey hotkey, string name)> _registered = new();
    private HwndSource? _source;
    private int _nextId = 1;

    public void Initialize()
    {
        if (_source != null) return;

        var p = new HwndSourceParameters("MouseHighlighterPro_HotkeySink")
        {
            Width = 0,
            Height = 0,
            PositionX = 0,
            PositionY = 0,
            WindowStyle = WS_POPUP,
        };

        _source = new HwndSource(p);
        _source.AddHook(WndProc);
    }

    public int Register(Hotkey hotkey, string name)
    {
        Initialize();
        if (_source is null) throw new InvalidOperationException("Hotkey sink not initialized.");

        int id = _nextId++;
        if (!RegisterHotKey(_source.Handle, id, hotkey.Modifiers, hotkey.Key))
            throw new InvalidOperationException($"RegisterHotKey failed. name={name}");

        _registered[id] = (hotkey, name);
        return id;
    }

    public void UnregisterAll()
    {
        if (_source is null) return;

        foreach (var id in _registered.Keys)
            UnregisterHotKey(_source.Handle, id);

        _registered.Clear();
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY)
        {
            int id = wParam.ToInt32();
            HotkeyPressed?.Invoke(this, id);
            handled = true;
        }
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        try { UnregisterAll(); } catch { /* ignore */ }
        if (_source != null)
        {
            _source.RemoveHook(WndProc);
            _source.Dispose();
            _source = null;
        }
    }

    #region Win32
    private const int WM_HOTKEY = 0x0312;
    private const int WS_POPUP = unchecked((int)0x80000000);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    #endregion
}
