using System.Runtime.InteropServices;

namespace Core_Logic.Infrastructure.Interop
{
    /// <summary>
    /// Represents the LASTINPUTINFO structure for retrieving the time of the last input event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        /// <summary>
        /// The size of the structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The tick count when the last input event was received.
        /// </summary>
        public uint dwTime;
    }
}