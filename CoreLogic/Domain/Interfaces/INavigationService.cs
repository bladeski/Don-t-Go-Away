using System;

namespace Core_Logic.Domain.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for a navigation service.
    /// </summary>
    public interface INavigationService<T>
    {
        /// <summary>
        /// Initializes the navigation service with the application's root frame.
        /// </summary>
        /// <param name="rootFrame">The root frame used for navigation.</param>
        void Initialize(T rootFrame);

        /// <summary>
        /// Navigates to the specified page type, optionally passing a parameter.
        /// </summary>
        /// <param name="pageType">The type of the page to navigate to.</param>
        /// <param name="parameter">An optional parameter to pass to the page.</param>
        /// <returns>True if navigation was successful; otherwise, false.</returns>
        bool Navigate(Type pageType, object? parameter = null);

        /// <summary>
        /// Navigates back to the previous page in the navigation stack, if possible.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Gets a value indicating whether navigation back is possible.
        /// </summary>
        bool CanGoBack { get; }
    }
}