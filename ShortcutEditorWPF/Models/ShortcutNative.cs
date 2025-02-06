using System;
using System.Globalization;
using ShellLink;

namespace ShortcutEditorWPF.Models;

public class ShortcutNative
{
    internal Shortcut InternalShortcut { get; set; }
    public string Link
    {
        get
        {
            if (InternalShortcut.StringData is not null && InternalShortcut.StringData.WorkingDir is not null)
                return InternalShortcut.StringData.WorkingDir;
            return "Nun";
        }
    }
    public string Attribute
    {
        get
        {
            if (InternalShortcut.ShellLinkHeader is not null && InternalShortcut.ShellLinkHeader.FileAttributes.ToString().Length > 1)
                return InternalShortcut.ShellLinkHeader.FileAttributes.ToString();
            return "Nun";
        }
    }
    public string Path
    {
        get
        {
            if (InternalShortcut.LinkInfo is not null && InternalShortcut.LinkInfo.CommonPathSuffix.Length > 1)
                return InternalShortcut.LinkInfo.CommonPathSuffix;
            return "Nun";
        }
    }
    public string TargetUnicode
    {
        get
        {
            if (InternalShortcut.ExtraData is not null && InternalShortcut.ExtraData.EnvironmentVariableDataBlock is not null)
                return InternalShortcut.ExtraData.EnvironmentVariableDataBlock.TargetUnicode;
            return "Nun";
        }
    }
    public string NetName => InternalShortcut.LinkInfo?.CommonNetworkRelativeLink?.NetName ?? "Nun";
    public string DeviceName => InternalShortcut.LinkInfo?.CommonNetworkRelativeLink?.DeviceName ?? "Nun";

    public string CreationTime
    {
        get
        {
            if(InternalShortcut.CreationTime > 0)
            {
                var unixDate = InternalShortcut.CreationTime;
                DateTime start = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Local);
                DateTime date= start.AddTicks(unixDate);
                return date.ToString(CultureInfo.CurrentCulture);
            }
            return "Nun";
        }
    }

    public ShortcutNative(Shortcut internalShortcut)
    {
        InternalShortcut = internalShortcut;
    }
}