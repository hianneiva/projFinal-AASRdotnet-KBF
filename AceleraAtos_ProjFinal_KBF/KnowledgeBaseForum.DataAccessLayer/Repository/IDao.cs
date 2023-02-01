namespace KnowledgeBaseForum.DataAccessLayer.Repository
{
    public interface IDao<T>
    {
        /// <summary>
        /// Selects all entries from a certain table.
        /// </summary>
        /// <returns>All elements from said table.</returns>
        Task<IEnumerable<T>> All();

        /// <summary>
        /// Get one entry from the table.
        /// </summary>
        /// <param name="id">The row Guid.</param>
        /// <returns>The specific selected entry from the table.</returns>
        Task<T?> Get(Guid id);

        /// <summary>
        /// Adds an entry to the table.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        Task Add (T entity);

        /// <summary>
        /// Updates an entry in the table.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        Task Update (T entity);

        /// <summary>
        /// Removes an entry from the table.
        /// </summary>
        /// <param name="id">The row Guid.</param>
        Task Delete (Guid id);

        /// <summary>
        /// Removes an entry from the table.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        Task Delete(T entity);
    }
}
