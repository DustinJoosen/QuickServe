using QuickServe.Entities;

namespace QuickServe.Services.Interfaces
{
    public interface IDataService
    {
        /// <summary>
        /// Delete the data file
        /// </summary>
        /// <param name="app">App to delete the file from.</param>
        void DeleteDataFile(App app);

        /// <summary>
        /// Gets file content of the app.
        /// </summary>
        /// <param name="appId">App to get contents from</param>
        /// <returns>File contents</returns>
        public string? Get(Guid appId);

        /// <summary>
        /// Sets file content of the app.
        /// </summary>
        /// <param name="appId">App to set contents from</param>
        /// <param name="contents">Contents to set</param>
        /// <returns>Boolean determining success</returns>
        public bool Set(Guid appId, string contents);
    }
}