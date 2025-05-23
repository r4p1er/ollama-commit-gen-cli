using OllamaCommitGen.Cli.Models;
using System.Text.Json;

namespace OllamaCommitGen.Cli.Utils
{
    public static class ConfigLoader
    {
        public static AppConfig LoadAppConfig(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"Configuration file '{path}' does not exist");
            var extension = Path.GetExtension(path).ToLowerInvariant();

            return extension switch
            {
                ".json" => LoadJsonConfig(path),
                _ => throw new NotSupportedException($"Unsupported config format: {extension}")
            };
        }

        private static AppConfig LoadJsonConfig(string path)
        {
            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<AppConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Failed to parse JSON configuration");

            return config;
        }
    }
}
