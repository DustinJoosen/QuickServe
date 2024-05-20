using Microsoft.AspNetCore.Mvc;
using QuickServe.Entities;

namespace QuickServe.Services.Interfaces
{
    public interface IFileService : IBaseService<Entities.File>
    {
        /// <summary>
        /// Uploads a given file.
        /// </summary>
        /// <param name="appId">App to link the file to. Is used for determining path.</param>
        /// <param name="formFile">File to be uploaded.</param>
        /// <returns>Filename of the uploaded file.</returns>
        public string Upload(Guid appId, IFormFile formFile);

        /// <summary>
        /// Removes a given file.
        /// </summary>
        /// <param name="file">File to remove.</param>
        public void RemoveFile(Entities.File file);

        /// <summary>
        /// Finds a file entity based on the app and the filename.
        /// </summary>
        /// <param name="appId">AppId to search within</param>
        /// <param name="fileName">File name to search for</param>
        /// <returns>Found file entity / null</returns>
        public Entities.File? Find(Guid appId, string fileName);
        
        /// <summary>
        /// Returns an (open) file stream from the given file.
        /// </summary>
        /// <param name="file">File to open</param>
        /// <returns>Opened stream</returns>
        public Stream GetStream(Entities.File file);

        /// <summary>
        /// Gets all files associated with an app.
        /// </summary>
        /// <param name="app">App to check</param>
        /// <returns>Lit of all files associated with an app.</returns>
        public List<Entities.File> GetAppFiles(App app);

        /// <summary>
        /// Removes all files associated with an app.
        /// </summary>
        /// <param name="app">App to check</param>
        public void RemoveAppFiles(App app);
    }
}
