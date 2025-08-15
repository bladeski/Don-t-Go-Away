using System.Runtime.InteropServices;

namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Provides native methods and safe wrappers for Windows interop.
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint SetWindowLongPtr(nint hWnd, int nIndex, nint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint GetWindowLongPtr(nint hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint CallWindowProc(nint lpPrevWndFunc, nint hWnd, uint msg, nint wParam, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA data);
    }
}