using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PrivateLibrary.DAL.Repositories.Interfaces
{
    public interface IEntityBaseRepository<in TKey, TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// Add new element.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains the entity that was created.</returns>
        /// <exception cref="DbUpdateException">An exception that is thrown when an error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">If a concurrency violation is encountered while saving to database.</exception>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Runs operation in transaction.
        /// </summary>
        /// <param name="operation">Method that represents the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<TEntity> RunInTransactionAsync(Func<Task<TEntity>> operation);

        /// <summary>
        /// Update information about element.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains the entity that was updated.</returns>
        /// <exception cref="DbUpdateException">An exception that is thrown when an error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">If a concurrency violation is encountered while saving to database.</exception>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Delete element.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        /// <exception cref="DbUpdateException">An exception that is thrown when an error is encountered while saving to the database.</exception>
        /// <exception cref="DbUpdateConcurrencyException">If a concurrency violation is encountered while saving to database.</exception>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Get all elements.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains a <see cref="IEnumerable{T}"/> that contains elements.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Get element by Id.
        /// </summary>
        /// <param name="id">Key in database.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains an entity that was found, or null.</returns>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        /// Get elements by a specific filter.
        /// </summary>
        /// <param name="predicate">Filter with key.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains a <see cref="IEnumerable{T}"/> that contains elements.</returns>
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get elements by a specific filter with no tracking.
        /// </summary>
        /// <param name="predicate">Filter with key.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains a <see cref="IEnumerable{T}"/> that contains elements.</returns>
        IQueryable<TEntity> GetByFilterNoTracking(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get the amount of elements with filter or without it.
        /// </summary>
        /// <param name="where">Filter.</param>
        /// <returns>>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains an amount of found elements.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? where = null);

        /// <summary>
        /// Get ordered, filtered list of elements.
        /// </summary>
        /// <typeparam name="TOrderKey">The type that we want to order list with.</typeparam>
        /// <param name="skip">How many records we want tp skip.</param>
        /// <param name="take">How many records we want to take.</param>
        /// <param name="includeProperties">What Properties we want to include to objects that we will receive.</param>
        /// <param name="where">Filter.</param>
        /// <param name="orderBy">Filter that defines by wich property we want to order by.</param>
        /// <param name="ascending">Ascending or descending ordering.</param>
        /// <returns>>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.
        /// The task result contains an ordered, filtered <see cref="IQueryable{T}"/>.</returns>
        IQueryable<TEntity> Get<TOrderKey>(int skip = 0, int take = 0, Expression<Func<TEntity, bool>>? where = null, Expression<Func<TEntity, TOrderKey>>? orderBy = null, bool ascending = true);
    }
}
