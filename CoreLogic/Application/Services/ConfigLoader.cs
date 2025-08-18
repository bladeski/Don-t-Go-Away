using Core_Logic.Domain.Interfaces;
using System.Text.Json;

namespace Core_Logic.Application.Services
{
    /// <summary>
    /// Provides instance methods for loading and saving configuration objects.
    /// </summary>
    public class ConfigLoader : IConfigLoader
    {
        /// <summary>
        /// Loads a configuration object from the specified file path.
        /// Returns a new instance with defaults if the file is missing or invalid.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>The loaded configuration object.</returns>
        public T Load<T>(string path) where T : new()
        {
            try
            {
                if (!File.Exists(path))
                    return new T();

                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// Saves a configuration object to the specified file path.
        /// </summary>
        /// <typeparam name="T">The configuration type.</typeparam>
        /// <param name="config">The configuration object to save.</param>
        /// <param name="path">The path to the configuration file.</param>
        public void Save<T>(T config, string path)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}