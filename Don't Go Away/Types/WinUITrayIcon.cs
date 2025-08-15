using Dont_Go_Away.Helpers;
using Dont_Go_Away.Interop;
using Dont_Go_Away.Services;
using Dont_Go_Away.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WinRT.Interop;

namespace Dont_Go_Away.Types
{
    /// <summary>
    /// Manages the application's tray icon, including creation, updates, notifications, and window interactions.
    /// </summary>
    public class WinUITrayIcon
    {
        // ─────────────────────────────────────────────────────────────
        // 🧠 Native Interop
        // ─────────────────────────────────────────────────────────────

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern bool Shell_NotifyIcon(uint dwMessage, ref NOTIFYICONDATA lpData);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(nint hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        // ─────────────────────────────────────────────────────────────
        // 🔧 Fields
        // ─────────────────────────────────────────────────────────────

        private readonly Window _window;
        private readonly nint _hWnd;
        private readonly uint _uID = 0x1;
        private readonly string _tooltip;
        private readonly Icon _icon;
        private readonly Action _onDoubleClick;

        private WndProcDelegate? _newWndProc;
        private nint _oldWndProc;

        private delegate nint WndProcDelegate(nint hWnd, uint msg, nint wParam, nint lParam);

        // ─────────────────────────────────────────────────────────────
        // 🚀 Constructor
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Initializes a new instance of the <see cref="WinUITrayIcon"/> class.
        /// </summary>
        /// <param name="window">The main application window.</param>
        /// <param name="icon">The tray icon.</param>
        /// <param name="tooltip">The tooltip text.</param>
        /// <param name="onDoubleClick">Action to perform on double-click.</param>
        /// <exception cref="ArgumentNullException">Thrown if any required argument is null.</exception>
        public WinUITrayIcon(Window window, Icon icon, string tooltip, Action onDoubleClick)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _tooltip = tooltip ?? string.Empty;
            _icon = icon ?? throw new ArgumentNullException(nameof(icon));
            _onDoubleClick = onDoubleClick ?? throw new ArgumentNullException(nameof(onDoubleClick));

            _hWnd = WinUIInterop.GetWindowHandle(window);
            AddTrayIcon();
            SubclassWindow();
        }

        // ─────────────────────────────────────────────────────────────
        // 🧩 Tray Icon Management
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Adds the tray icon to the system tray.
        /// </summary>
        private void AddTrayIcon()
        {
            var data = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
                hWnd = _hWnd,
                uID = _uID,
                uFlags = NotifyIconFlags.NIF_MESSAGE | NotifyIconFlags.NIF_ICON | NotifyIconFlags.NIF_TIP,
                uCallbackMessage = Win32Constants.WM_TRAYICON,
                hIcon = _icon.Handle,
                szTip = _tooltip
            };

            if (!Shell_NotifyIcon(NotifyIconConstants.NIM_ADD, ref data))
            {
                Logger.LogError("Failed to add tray icon.");
            }
        }

        /// <summary>
        /// Removes the tray icon from the system tray.
        /// </summary>
        public void RemoveTrayIcon()
        {
            var data = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
                hWnd = _hWnd,
                uID = _uID
            };

            if (!Shell_NotifyIcon(NotifyIconConstants.NIM_DELETE, ref data))
            {
                Logger.LogError("Failed to remove tray icon.");
            }
        }

        /// <summary>
        /// Updates the tray icon image.
        /// </summary>
        /// <param name="newIcon">The new icon to display.</param>
        public void UpdateIcon(Icon newIcon)
        {
            if (newIcon == null) return;

            var iconData = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
                hWnd = _hWnd,
                uID = _uID,
                uFlags = NotifyIconFlags.NIF_ICON,
                hIcon = newIcon.Handle
            };

            if (!Shell_NotifyIcon(NotifyIconConstants.NIM_MODIFY, ref iconData))
            {
                Logger.LogError("Failed to update tray icon.");
            }
        }

        /// <summary>
        /// Updates the tray icon tooltip.
        /// </summary>
        /// <param name="tooltip">The new tooltip text.</param>
        public void UpdateIconTooltip(string tooltip)
        {
            var iconData = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
                hWnd = _hWnd,
                uID = _uID,
                uFlags = NotifyIconFlags.NIF_TIP,
                szTip = tooltip
            };

            if (!Shell_NotifyIcon(NotifyIconConstants.NIM_MODIFY, ref iconData))
            {
                Logger.LogError("Failed to update tray tooltip.");
            }
        }

        /// <summary>
        /// Shows a balloon tip notification from the tray icon.
        /// </summary>
        /// <param name="title">The title of the balloon tip.</param>
        /// <param name="message">The message of the balloon tip.</param>
        public void ShowBalloonTip(string title, string message)
        {
            var data = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf<NOTIFYICONDATA>(),
                hWnd = _hWnd,
                uID = _uID,
                uFlags = NotifyIconFlags.NIF_INFO,
                szInfo = message,
                szInfoTitle = title,
                dwInfoFlags = NotifyIconFlags.NIF_INFO
            };

            if (!Shell_NotifyIcon(NotifyIconConstants.NIM_MODIFY, ref data))
            {
                Logger.LogError("Failed to show balloon tip.");
            }
        }

        // ─────────────────────────────────────────────────────────────
        // 🪟 Window Subclassing
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Subclasses the window to handle tray icon messages.
        /// </summary>
        private void SubclassWindow()
        {
            try
            {
                _newWndProc = CustomWndProc;
                var newPtr = Marshal.GetFunctionPointerForDelegate(_newWndProc);
                _oldWndProc = NativeMethods.GetWindowLongPtr(_hWnd, Win32Constants.GWLP_WNDPROC);
                NativeMethods.SetWindowLongPtr(_hWnd, Win32Constants.GWLP_WNDPROC, newPtr);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to subclass window", ex);
            }
        }

        /// <summary>
        /// Custom window procedure for tray icon events and window messages.
        /// Handles double-click, balloon tip click, context menu, close, and minimize events.
        /// </summary>
        private nint CustomWndProc(nint hWnd, uint msg, nint wParam, nint lParam)
        {
            try
            {
                if (msg == Win32Constants.WM_TRAYICON)
                {
                    switch ((int)lParam)
                    {
                        case Win32Constants.WM_LBUTTONDBLCLK:
                            _onDoubleClick?.Invoke();
                            break;
                        case Win32Constants.NIN_BALLOONUSERCLICK:
                            BringMainWindowToFront();
                            break;
                        case 0x0201: // WM_LBUTTONDOWN
                            ShowContextMenu();
                            break;
                    }
                }
                else if (msg == Win32Constants.WM_CLOSE)
                {
                    _window.DispatcherQueue.TryEnqueue(async () =>
                    {
                        var result = await ShowCloseConfirmationAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            App.MainWindowInstance?.Close();
                        }
                    });
                    return nint.Zero; // Prevent default close
                }
                else if (msg == Win32Constants.WM_SYSCOMMAND && (int)wParam == Win32Constants.SC_MINIMIZE)
                {
                    _window.DispatcherQueue.TryEnqueue(() => HideMainWindow());
                    return nint.Zero; // Prevent default minimize
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("CustomWndProc error", ex);
            }

            return NativeMethods.CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        /// <summary>
        /// Shows the context menu for the tray icon.
        /// </summary>
        private void ShowContextMenu()
        {
            var hMenu = CreatePopupMenu();
            AppendMenu(hMenu, Win32Constants.MF_STRING, 1, "Show Window");
            AppendMenu(hMenu, Win32Constants.MF_STRING, 2, "Settings");
            var driftLabel = DriftService.Instance?.IsRunning == true ? "Stop Service" : "Start Service";
            AppendMenu(hMenu, Win32Constants.MF_STRING, 3, driftLabel);
            AppendMenu(hMenu, Win32Constants.MF_STRING, 4, "Exit");

            var pt = new POINT();
            NativeMethods.GetCursorPos(out pt);

            var selected = TrackPopupMenu(hMenu, Win32Constants.TPM_LEFTALIGN | Win32Constants.TPM_RETURNCMD, pt.X, pt.Y, 0, _hWnd, IntPtr.Zero);

            switch (selected)
            {
                case 1:
                    _window.DispatcherQueue.TryEnqueue(() => App.MainWindowInstance?.Activate());
                    break;
                case 2:
                    _window.DispatcherQueue.TryEnqueue(() =>
                    {
                        App.MainWindowInstance?.Activate();
                        NavigationService.Navigate(typeof(SettingsPage));
                    });
                    break;
                case 3:
                    _window.DispatcherQueue.TryEnqueue(() => DriftService.Instance?.Toggle());
                    break;
                case 4:
                    _window.DispatcherQueue.TryEnqueue(async () =>
                    {
                        var result = await ShowCloseConfirmationAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            App.MainWindowInstance?.Activate();
                            App.MainWindowInstance?.Close();
                        }
                    });
                    break;
            }
        }

        /// <summary>
        /// Shows a confirmation dialog before closing the application.
        /// </summary>
        /// <returns>The result of the dialog.</returns>
        private async Task<ContentDialogResult> ShowCloseConfirmationAsync()
        {
            var dialog = new ContentDialog
            {
                Title = "Exit Don't Go Away?",
                Content = "Are you sure you want to close the app? It will stop preventing idle.",
                PrimaryButtonText = "Exit",
                CloseButtonText = "Cancel",
                XamlRoot = _window.Content.XamlRoot
            };

            return await dialog.ShowAsync();
        }

        // ─────────────────────────────────────────────────────────────
        // 🧭 Window Control
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Hides the main window and shows a tray notification.
        /// </summary>
        public void HideMainWindow()
        {
            try
            {
                var hWnd = _window.GetWindowHandle();
                NativeMethods.ShowWindow(hWnd, Win32Constants.SW_HIDE);
                ShowBalloonTip("Don't Go Away", "Don't Go Away is running in the tray");
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to hide main window", ex);
            }
        }

        /// <summary>
        /// Brings the main window to the foreground.
        /// </summary>
        private static void BringMainWindowToFront()
        {
            try
            {
                var hwnd = WindowNative.GetWindowHandle(App.MainWindowInstance);
                ShowWindow(hwnd, Win32Constants.SW_RESTORE);
                SetForegroundWindow(hwnd);
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to bring window to front", ex);
            }
        }
    }
}