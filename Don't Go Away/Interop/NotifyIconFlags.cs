namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Flags for NOTIFYICONDATA structure to specify which members are valid.
    /// </summary>
    public static class NotifyIconFlags
    {
        public const uint NIF_MESSAGE = 0x00000001;
        public const uint NIF_ICON = 0x00000002;
        public const uint NIF_TIP = 0x00000004;
        public const uint NIF_STATE = 0x00000008;
        public const uint NIF_INFO = 0x00000010;
        public const uint NIF_GUID = 0x00000020;
        public const uint NIF_REALTIME = 0x00000040;
        public const uint NIF_SHOWTIP = 0x00000080;
    }
}