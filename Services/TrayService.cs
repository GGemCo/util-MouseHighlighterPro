
using System;
using System.Drawing;
using System.Windows;
using MouseHighlighterPro.Models;

namespace MouseHighlighterPro.Services;

public sealed class TrayService : IDisposable
{
    private readonly System.Windows.Forms.NotifyIcon _notifyIcon;

    public event EventHandler? ToggleOverlayRequested;
    public event EventHandler? OpenSettingsRequested;
    public event EventHandler? ExitRequested;

    public TrayService()
    {
        _notifyIcon = new System.Windows.Forms.NotifyIcon
        {
            Text = "Mouse Highlighter Pro",
            Icon = SystemIcons.Application,
            Visible = true,
        };

        var menu = new System.Windows.Forms.ContextMenuStrip();
        var miToggle = new System.Windows.Forms.ToolStripMenuItem("오버레이 표시/숨김(&T)");
        var miSettings = new System.Windows.Forms.ToolStripMenuItem("설정(&S)");
        var miExit = new System.Windows.Forms.ToolStripMenuItem("종료(&X)");

        miToggle.Click += (_, _) => ToggleOverlayRequested?.Invoke(this, EventArgs.Empty);
        miSettings.Click += (_, _) => OpenSettingsRequested?.Invoke(this, EventArgs.Empty);
        miExit.Click += (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty);

        menu.Items.Add(miToggle);
        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add(miSettings);
        menu.Items.Add(new System.Windows.Forms.ToolStripSeparator());
        menu.Items.Add(miExit);

        _notifyIcon.ContextMenuStrip = menu;

        _notifyIcon.DoubleClick += (_, _) => OpenSettingsRequested?.Invoke(this, EventArgs.Empty);
    }

    public void SetOverlayEnabled(bool enabled)
    {
        _notifyIcon.Text = enabled ? "Mouse Highlighter Pro (ON)" : "Mouse Highlighter Pro (OFF)";
    }

    public void Dispose()
    {
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
    }
}
