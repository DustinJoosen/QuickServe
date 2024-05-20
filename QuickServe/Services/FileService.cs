using QuickServe.Entities;
using QuickServe.Services.Interfaces;

namespace QuickServe.Services
{
    public class FileService : BaseService<Entities.File>, IFileService
    {
        public FileService(IJsonService jsonService) : base(jsonService, "files.json")
        {
        }

        public Entities.File? Find(Guid appId, string fileName)
        {
            var file = this.GetAll()
                .FirstOrDefault(file => file.AppId == appId && file.FileName == fileName);

            return file;
        }

        public Stream GetStream(Entities.File file)
        {
            // Create the correct directory
            this.FetchOrCreateAppFileDirectory(file.AppId, out string directoryPath);
            string filePath = Path.Combine(directoryPath, file.FileName);

            FileStream fileStream = System.IO.File.OpenRead(filePath);
            return fileStream;
        }

        public string Upload(Guid appId, IFormFile formFile)
        {
            // Create the correct directory
            this.FetchOrCreateAppFileDirectory(appId, out string directoryPath);

            string fileName = formFile.FileName.Replace(" ", "_").ToLower();
            string filePath = Path.Combine(directoryPath, fileName);
            
            // Save the file to the directory
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            return fileName;
        }

        public void RemoveFile(Entities.File file)
        {
            // Create the correct directory
            this.FetchOrCreateAppFileDirectory(file.AppId, out string directoryPath);
            string filePath = Path.Combine(directoryPath, file.FileName);

            // Remove file.
            System.IO.File.Delete(filePath);
        }


        public List<Entities.File> GetAppFiles(App app)
        {
            var files = this.GetAll()
                .Where(file => file.AppId == app.Uuid).ToList();

            return files;
        }

        public void RemoveAppFiles(App app)
        {
            var files = this.GetAppFiles(app);

            foreach (var file in files)
            {
                // Delete file and entity.
                this.RemoveFile(file);
                this.Delete(file);
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
