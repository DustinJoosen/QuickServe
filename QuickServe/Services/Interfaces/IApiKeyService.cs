namespace QuickServe.Services.Interfaces
{
    public interface IApiKeyService
    {
        /// <summary>
        /// Generates a new API Key based on a guid, and then stores it as a valid key.
        /// </summary>
        /// <returns>Generated key</returns>
        public string GenerateApiKey();

        /// <summary>
        /// Determines wether a given key is valid.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Boolean determining wether the key is valid.</returns>
        public bool IsKeyValid(string key);

    }
}
