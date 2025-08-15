namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Contains Win32 and tray icon related constants for interop operations.
    /// </summary>
    public static class Win32Constants
    {
        /// <summary>Hides a window.</summary>
        public const int SW_HIDE = 0;
        /// <summary>Shows a window.</summary>
        public const int SW_SHOW = 5;
        /// <summary>Restores a minimized window.</summary>
        public const int SW_RESTORE = 9;

        /// <summary>Index for window procedure pointer.</summary>
        public const int GWLP_WNDPROC = -4;

        /// <summary>Base value for application-defined messages.</summary>
        public const int WM_APP = 0x8000;
        /// <summary>Message ID for tray icon events.</summary>
        public const int WM_TRAYICON = WM_APP + 1;
        /// <summary>Message ID for balloon tip user click.</summary>
        public const int NIN_BALLOONUSERCLICK = 0x0405;
        /// <summary>Message ID for left button double-click.</summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>Message ID for system command.</summary>
        public const int WM_SYSCOMMAND = 0x0112;
        /// <summary>System command for minimize.</summary>
        public const int SC_MINIMIZE = 0xF020;

        /// <summary>Menu flag for string type.</summary>
        public const uint MF_STRING = 0x00000000;
        /// <summary>Menu flag for left alignment.</summary>
        public const uint TPM_LEFTALIGN = 0x0000;
        /// <summary>Menu flag for return command.</summary>
        public const uint TPM_RETURNCMD = 0x0100;

        /// <summary>Message ID for user-defined messages.</summary>
        public const uint WM_USER = 0x0400;
        /// <summary>Message ID for window close.</summary>
        public const uint WM_CLOSE = 0x0010;
    }
}