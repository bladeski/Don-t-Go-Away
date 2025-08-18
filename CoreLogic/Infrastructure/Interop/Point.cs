// Don't Go Away\Interop\POINT.cs
using System.Runtime.InteropServices;

namespace Core_Logic.Infrastructure.Interop
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