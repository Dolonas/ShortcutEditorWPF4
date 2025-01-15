using ShellLink;

namespace ShortcutEditorWPF.Models;

public class ShortcutNative
{
    internal Shortcut InternalShortcut { get; set; }

    public ShortcutNative(Shortcut internalShortcut)
    {
        InternalShortcut = internalShortcut;
    }

    public ShortcutNative() : this(new Shortcut())
    {
    }
}