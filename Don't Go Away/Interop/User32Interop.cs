using System.Runtime.InteropServices;

namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Provides P/Invoke declarations for commonly used User32.dll functions.
    /// </summary>
    public static class User32Interop
    {
        /// <summary>
        /// Retrieves the time of the last input event.
        /// </summary>
        /// <param name="plii">A reference to a <see cref="LASTINPUTINFO"/> structure.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Sets the cursor position to the specified screen coordinates.
        /// </summary>
        /// <param name="X">The new X-coordinate.</param>
        /// <param name="Y">The new Y-coordinate.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// Retrieves the current cursor position.
        /// </summary>
        /// <param name="lpPoint">A <see cref="POINT"/> structure that receives the screen coordinates.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
    }
}