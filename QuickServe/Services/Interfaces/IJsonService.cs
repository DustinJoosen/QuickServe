namespace QuickServe.Services.Interfaces
{
    public interface IJsonService
    {
        /// <summary>
        /// Gets the file contents of the specified file. It then maps it to the given type
        /// </summary>
        /// <typeparam name="T">Type to match the content to</typeparam>
        /// <param name="fileName">File to get</param>
        /// <returns>Mapped contents of file</returns>
        public T? GetFileContent<T>(string fileName);

        /// <summary>
        /// Sets the file contents of the specified file.
        /// </summary>
        /// <typeparam name="T">Type of the given value</typeparam>
        /// <param name="fileName">File to set the content</param>
        /// <param name="value">Value to set in the file</param>
        /// <returns>Boolean determining wether the process succeeded</returns>
        public bool SetFileContent<T>(string fileName, T value);
    }
}
