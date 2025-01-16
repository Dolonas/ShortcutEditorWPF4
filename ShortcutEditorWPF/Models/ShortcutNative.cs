using ShellLink;

namespace ShortcutEditorWPF.Models;

public class ShortcutNative
{
    internal Shortcut InternalShortcut { get; set; }
    public string Link
    {
        get
        {
            if (InternalShortcut.LinkInfo is not null)
                return InternalShortcut.StringData.WorkingDir;
            return "Nun";
        }
    }
    public string CreationTime
    {
        get
        {
            return InternalShortcut.CreationTime.ToString();
            return "Nun";
        }
    }

    public ShortcutNative(Shortcut internalShortcut)
    {
        InternalShortcut = internalShortcut;
    }
}