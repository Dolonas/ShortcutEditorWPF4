using ShellLink;

namespace ShortcutEditorWPF.Models;

public class ShortcutNative
{
    internal Shortcut InternalShortcut { get; set; }
    public string Link
    {
        get
        {
            if (InternalShortcut is not null && InternalShortcut.LinkInfo is not null)
                return InternalShortcut.LinkInfo.CommonPathSuffixUnicode;
            return "Nun";
        }
    }
    public string CreationTime
    {
        get
        {
            if (InternalShortcut is not null)
                return InternalShortcut.CreationTime.ToString();
            return "Nun";
        }
    }

    public ShortcutNative(Shortcut internalShortcut)
    {
        InternalShortcut = internalShortcut;
    }
}