namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Contains constants for tray icon operations and custom messages.
    /// </summary>
    public static class NotifyIconConstants
    {
        /// <summary>
        /// Adds a new icon to the system tray.
        /// </summary>
        public const uint NIM_ADD = 0x00000000;

        /// <summary>
        /// Modifies an existing icon in the system tray.
        /// </summary>
        public const uint NIM_MODIFY = 0x00000001;

        /// <summary>
        /// Deletes an icon from the system tray.
        /// </summary>
        public const uint NIM_DELETE = 0x00000002;

        /// <summary>
        /// Custom message ID for tray icon notifications.
        /// </summary>
        public const uint NOTIFY_ICON_MESSAGE = 0x0400 + 1024;
    }
}