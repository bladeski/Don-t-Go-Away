namespace Core_Logic.Domain.Interfaces
{
    /// <summary>
    /// Defines methods for loading and saving configuration objects.
    /// </summary>
    public interface IConfigLoader
    {
        /// <summary>
        /// Loads a configuration object from the specified file path.
        /// Returns a new instance with defaults if the file is missing or invalid.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>The loaded configuration object.</returns>
        T Load<T>(string path) where T : new();

        /// <summary>
        /// Saves a configuration object to the specified file path.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="config">The configuration object to save.</param>
        /// <param name="path">The path to the configuration file.</param>
        void Save<T>(T config, string path);
    }
}