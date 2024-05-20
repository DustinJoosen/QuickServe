using QuickServe.Entities;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public class DataService : IDataService
    {
        private static string DataFileName = "_data.txt";

        public string? Get(Guid appId)
        {
            // Create the correct directory
            this.FetchOrCreateAppFileDirectory(appId, out string directoryPath);
            string filePath = Path.Combine(directoryPath, DataService.DataFileName);

            try
            {
                return System.IO.File.ReadAllText(filePath);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not get dataservice file");
                return null;
            }

        }

        public bool Set(Guid appId, string contents)
        {
            // Create the correct directory.
            this.FetchOrCreateAppFileDirectory(appId, out string directoryPath);
            string filePath = Path.Combine(directoryPath, DataService.DataFileName);

            // Make sure the _data file exists.
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Close();
            }

            // Write contents to data file.
            try
            {
                System.IO.File.WriteAllText(filePath, contents);
                return true;
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not set dataservice file");
                return false;
            }
        }

        public void DeleteDataFile(Entities.App app)
        {
            // Get the correct directory.
            this.FetchOrCreateAppFileDirectory(app.Uuid, out string directoryPath);
            string filePath = Path.Combine(directoryPath, DataService.DataFileName);

            try
            {
                System.IO.File.Delete(filePath);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not set dataservice file");
            }
        }

        private void FetchOrCreateAppFileDirectory(Guid appId, out string directoryPath)
        {
            directoryPath = $"Data/Storage/{appId}/";
            if (Directory.Exists(directoryPath))
                return;

            Directory.CreateDirectory(directoryPath);
        }
    }
}
