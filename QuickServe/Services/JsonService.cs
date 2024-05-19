using Microsoft.Extensions.FileProviders.Composite;
using QuickServe.Services.Interfaces;
using System.Text.Json;

namespace QuickServe.Services
{
    public class JsonService : IJsonService
    {
        private static JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public T? GetFileContent<T>(string fileName)
        {
            string filePath = $"Data/Storage/{fileName}";

            try
            {
                var contents = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(contents, JsonService.jsonSerializerOptions);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not get file contents");
                return default;
            }
        }

        public bool SetFileContent<T>(string fileName, T value)
        {
            string filePath = $"Data/Storage/{fileName}";

            try
            {
                string json = JsonSerializer.Serialize(value, JsonService.jsonSerializerOptions);
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not set file contents");
                return false;
            }
        }
    }
}
