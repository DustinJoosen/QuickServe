using QuickServe.Entities;

namespace QuickServe.Services.Interfaces
{
    public interface IBaseService<T> where T : IdentifiableEntity
    {
        /// <summary>
        /// Retrieves all entities found.
        /// </summary>
        /// <returns>A list of all items found</returns>
        public List<T> GetAll();

        /// <summary>
        /// Retrieves a specific entity
        /// </summary>
        /// <param name="uuid">Guid of entity to find</param>
        /// <returns>The found item</returns>
        public T? GetById(Guid uuid);

        /// <summary>
        /// Attempts to create an entity
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns>The created entity</returns>
        public T? Create(T entity);

        /// <summary>
        /// Attempts to update an entity
        /// </summary>
        /// <param name="uuid">Guid of entity to update</param>
        /// <param name="entity">New value of entity</param>
        /// <returns>Boolean determining wether the process succeeded</returns>
        public bool Update(Guid uuid, T entity);

        /// <summary>
        /// Attempts to delete an entity
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>Boolean determining wether the process succeeded</returns>
        public bool Delete(T entity);

    }
}
