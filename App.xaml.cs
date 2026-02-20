
using System;
using System.Windows;
using MouseHighlighterPro.Models;
using MouseHighlighterPro.Services;
using MouseHighlighterPro.Windows;

namespace MouseHighlighterPro;

public partial class App : System.Windows.Application
{
    private AppHost? _host;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _host = new AppHost();
        _host.Start();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        try { _host?.Dispose(); } catch { /* ignore */ }
        base.OnExit(e);
    }
}
