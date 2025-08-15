using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace Dont_Go_Away.Helpers
{
    /// <summary>
    /// Provides extension methods for <see cref="Window"/>.
    /// </summary>
    public static class WindowExtensions
    {
        public static nint GetWindowHandle(this Window window)
        {
            return WindowNative.GetWindowHandle(window);
        }
    }
}