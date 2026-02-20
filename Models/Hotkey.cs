
namespace MouseHighlighterPro.Models;

public readonly record struct Hotkey(uint Modifiers, uint Key)
{
    public static Hotkey DefaultToggleOverlay => new(Modifiers: (uint)HotkeyModifiers.Control | (uint)HotkeyModifiers.Alt, Key: (uint)VirtualKey.M);
    public static Hotkey DefaultOpenSettings => new(Modifiers: (uint)HotkeyModifiers.Control | (uint)HotkeyModifiers.Alt, Key: (uint)VirtualKey.O);
}

[System.Flags]
public enum HotkeyModifiers : uint
{
    None = 0,
    Alt = 0x0001,
    Control = 0x0002,
    Shift = 0x0004,
    Win = 0x0008,
}

public enum VirtualKey : uint
{
    M = 0x4D,
    O = 0x4F,
}
