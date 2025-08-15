using Microsoft.UI.Xaml.Controls;
using System;

namespace Dont_Go_Away.Services
{
    /// <summary>
    /// Provides centralized navigation functionality for the application's main frame.
    /// </summary>
    public static class NavigationService
    {
        private static Frame? _rootFrame;

        /// <summary>
        /// Initializes the navigation service with the application's root frame.
        /// </summary>
        /// <param name="rootFrame">The root <see cref="Frame"/> used for navigation.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rootFrame"/> is null.</exception>
        public static void Initialize(Frame rootFrame)
        {
            _rootFrame = rootFrame ?? throw new ArgumentNullException(nameof(rootFrame));
        }

        /// <summary>
        /// Navigates to the specified page type, optionally passing a parameter.
        /// </summary>
        /// <param name="pageType">The type of the page to navigate to.</param>
        /// <param name="parameter">An optional parameter to pass to the page.</param>
        /// <returns>True if navigation was successful; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the navigation service is not initialized.</exception>
        public static bool Navigate(Type pageType, object? parameter = null)
        {
            if (_rootFrame == null)
                throw new InvalidOperationException("NavigationService not initialized.");

            return _rootFrame.Navigate(pageType, parameter);
        }

        /// <summary>
        /// Navigates back to the previous page in the navigation stack, if possible.
        /// </summary>
        public static void GoBack()
        {
            if (_rootFrame?.CanGoBack == true)
                _rootFrame.GoBack();
        }

        /// <summary>
        /// Gets a value indicating whether navigation back is possible.
        /// </summary>
        public static bool CanGoBack => _rootFrame?.CanGoBack == true;
    }
}