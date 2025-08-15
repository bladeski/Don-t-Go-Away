using Dont_Go_Away.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace Dont_Go_Away.Interop
{
    /// <summary>
    /// Provides WinUI and Windows interop helpers for window positioning and screen size.
    /// </summary>
    public static class WinUIInterop
    {
        /// <summary>
        /// Gets the window handle for a WinUI <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The window instance.</param>
        /// <returns>The native window handle.</returns>
        /// <exception cref="ArgumentNullException">Thrown if window is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if handle retrieval fails.</exception>
        public static nint GetWindowHandle(Window window)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window), "Window cannot be null.");
            }

            nint hWnd = WindowNative.GetWindowHandle(window);
            if (hWnd == nint.Zero)
            {
                throw new InvalidOperationException("Failed to retrieve window handle.");
            }

            return hWnd;
        }

        [DllImport("user32.dll")]
        public static extern nint GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(
            nint hWnd,
            nint hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        /// <summary>Constant for topmost window positioning.</summary>
        public static readonly nint HWND_TOP = nint.Zero;
        /// <summary>Flag to prevent z-order change.</summary>
        public const uint SWP_NOZORDER = 0x0004;
        /// <summary>Flag to prevent window activation.</summary>
        public const uint SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// Centers the specified window on the screen.
        /// </summary>
        /// <param name="window">The window to center.</param>
        /// <param name="windowWidth">Window width in pixels.</param>
        /// <param name="windowHeight">Window height in pixels.</param>
        public static void CenterWindow(Window window, int windowWidth, int windowHeight)
        {
            try
            {
                nint hwnd = GetWindowHandle(window);
                var (screenWidth, screenHeight) = GetScreenSize();

                int x = (screenWidth - windowWidth) / 2;
                int y = (screenHeight - windowHeight) / 2;

                bool success = SetWindowPos(
                    hwnd,
                    HWND_TOP,
                    x, y,
                    windowWidth, windowHeight,
                    SWP_NOZORDER | SWP_NOACTIVATE);

                if (!success)
                {
                    int error = Marshal.GetLastWin32Error();
                    Logger.LogError($"SetWindowPos failed with error code: {error}");
                    throw new InvalidOperationException($"SetWindowPos failed. Win32 Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("CenterWindow error", ex);
                throw;
            }
        }

        [DllImport("user32.dll")]
        public static extern nint GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern nint GetWindowDC(nint hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(nint hWnd, nint hDC);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(nint hdc, int nIndex);

        /// <summary>Device cap index for horizontal resolution.</summary>
        public const int HORZRES = 8;
        /// <summary>Device cap index for vertical resolution.</summary>
        public const int VERTRES = 10;

        /// <summary>
        /// Gets the screen size in pixels.
        /// </summary>
        /// <returns>A tuple containing screen width and height.</returns>
        /// <exception cref="InvalidOperationException">Thrown if screen size cannot be determined.</exception>
        public static (int screenWidth, int screenHeight) GetScreenSize()
        {
            nint hWnd = GetDesktopWindow();
            if (hWnd == nint.Zero)
            {
                throw new InvalidOperationException("Failed to get desktop window handle.");
            }

            nint hDC = GetWindowDC(hWnd);
            if (hDC == nint.Zero)
            {
                throw new InvalidOperationException("Failed to get device context for desktop window.");
            }

            try
            {
                int width = GetDeviceCaps(hDC, HORZRES);
                int height = GetDeviceCaps(hDC, VERTRES);

                if (width <= 0 || height <= 0)
                {
                    throw new InvalidOperationException("Invalid screen dimensions retrieved.");
                }

                return (width, height);
            }
            finally
            {
                ReleaseDC(hWnd, hDC);
            }
        }
    }
}