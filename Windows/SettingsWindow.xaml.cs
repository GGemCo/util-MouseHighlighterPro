
using System.ComponentModel;
using System.Windows;
using MouseHighlighterPro.Rendering;
using MouseHighlighterPro.ViewModels;

namespace MouseHighlighterPro.Windows;

public partial class SettingsWindow : Window
{
    private readonly SettingsViewModel _vm;

    public SettingsWindow(SettingsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = vm;

        vm.SaveRequested += (_, _) => { DialogResult = true; Close(); };
        vm.CancelRequested += (_, _) => { DialogResult = false; Close(); };

        Loaded += (_, _) =>
        {
            _vm.PropertyChanged += OnVmPropertyChanged;
            PreviewSurface.SizeChanged += (_, _) => UpdatePreview();
            UpdatePreview();
        };

        Unloaded += (_, _) =>
        {
            _vm.PropertyChanged -= OnVmPropertyChanged;
        };
    }

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Any visual-affecting setting change should refresh preview.
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (PreviewSurface is null) return;
        if (PreviewSurface.ActualWidth <= 1 || PreviewSurface.ActualHeight <= 1) return;

        var center = new System.Windows.Point(PreviewSurface.ActualWidth / 2, PreviewSurface.ActualHeight / 2);

        PreviewSurface.Update(new OverlayFrame
        {
            CursorDip = center,
            HasCursor = true,
            Settings = _vm.Settings,
            NowMs = Environment.TickCount64,
            LastClickMs = null,
            LastClickButton = null
        });
    }
}
