
using System;
using System.Diagnostics;
using System.Windows.Media;

namespace MouseHighlighterPro.Services;

public sealed class RenderLoop : IDisposable
{
    private readonly Action _tick;
    private bool _running;
    private long _lastTick;
    private int _targetFps;

    public RenderLoop(Action tick, int targetFps)
    {
        _tick = tick ?? throw new ArgumentNullException(nameof(tick));
        _targetFps = Math.Clamp(targetFps, 15, 240);
    }

    public void SetTargetFps(int fps) => _targetFps = Math.Clamp(fps, 15, 240);

    public void Start()
    {
        if (_running) return;
        _running = true;
        _lastTick = Stopwatch.GetTimestamp();
        CompositionTarget.Rendering += OnRendering;
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;
        CompositionTarget.Rendering -= OnRendering;
    }

    private void OnRendering(object? sender, EventArgs e)
    {
        if (!_running) return;

        var now = Stopwatch.GetTimestamp();
        var elapsed = (now - _lastTick) / (double)Stopwatch.Frequency;
        var minInterval = 1.0 / _targetFps;

        if (elapsed < minInterval)
            return;

        _lastTick = now;
        _tick();
    }

    public void Dispose() => Stop();
}
