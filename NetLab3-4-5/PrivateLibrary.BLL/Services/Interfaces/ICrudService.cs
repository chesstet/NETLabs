using PrivateLibrary.BLL.Common;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    public interface ICrudService<in TKey, TEntity>
        where TEntity : class?, new()
    {
        /// <summary>
        /// Add entity to the database.
        /// </summary>
        /// <param name="dto">Entity to add.</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.
        /// The task result contains the entity that was created.</returns>
        Task<Result<TEntity>> CreateAsync(TEntity dto);

        /// <summary>
        /// Get entity by it's key.
        /// </summary>
        /// <param name="id">Key in the table.</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.
        /// The task result contains the entity that was found, or null.</returns>
        Task<TEntity> GetByIdAsync(TKey id);

        /// <summary>
        /// Update existing entity in the database.
        /// </summary>
        /// <param name="dto">Entity that will be to updated.</param>
        /// <returns>A <see cref="Task{TEntity}"/> representing the result of the asynchronous operation.
        /// The task result contains the entity that was updated.</returns>
        Task<Result<TEntity>> UpdateAsync(TEntity dto);

        /// <summary>
        ///  Delete entity.
        /// </summary>
        /// <param name="id">Key in the table.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RemoveAsync(TKey id);
    }
}
