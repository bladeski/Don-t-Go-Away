// Don't Go Away\Interop\POINT.cs
using System.Runtime.InteropServices;

namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Represents a point (x, y) in screen coordinates for Win32 API interop.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>
        /// The horizontal (X) position.
        /// </summary>
        public int X;

        /// <summary>
        /// The vertical (Y) position.
        /// </summary>
        public int Y;
    }
}