using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.DAL.Repositories.Realizations
{
    /// <summary>
    /// Repository for accessing the database.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Entity type.</typeparam>
    /// /// <typeparam name="TDbContext">DbContext type.</typeparam>
    public class EntityBaseRepository<TKey, TValue, TDbContext> : IEntityBaseRepository<TKey, TValue>
        where TValue : class, new() where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;
        protected readonly DbSet<TValue> DbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBaseRepository{TKey, TValue, TDbContext}"/> class.
        /// </summary>
        /// <param name="dbContext">TDbContext.</param>
        protected EntityBaseRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TValue>();
        }

        /// <inheritdoc/>
        public virtual async Task<TValue> CreateAsync(TValue entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return await Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TValue> RunInTransactionAsync(Func<Task<TValue>> operation)
        {
            ArgumentNullException.ThrowIfNull(operation, nameof(operation));
            await using var transaction = await DbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await operation();
                await transaction.CommitAsync();

                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc/>
        public virtual async Task RemoveAsync(TValue entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            DbContext.Entry(entity).State = EntityState.Deleted;
            
            await DbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TValue>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TValue>> GetByFilterAsync(Expression<Func<TValue, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
            var query = DbSet.Where(predicate);

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual IQueryable<TValue> GetByFilterNoTracking(Expression<Func<TValue, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
            var query = DbSet.Where(predicate);

            return query.AsNoTracking();
        }

        /// <inheritdoc/>
        public virtual async Task<TValue?> GetByIdAsync(TKey id) => await DbSet.FindAsync(id).AsTask();

        /// <inheritdoc/>
        public virtual async Task<TValue> UpdateAsync(TValue entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            DbContext.Entry(entity).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<int> CountAsync(Expression<Func<TValue, bool>>? where = null)
        {
            return await (where == null
                   ? DbSet.CountAsync()
                   : DbSet.Where(where).CountAsync());
        }

        /// <inheritdoc/>
        public virtual IQueryable<TValue> Get<TOrderKey>(
            int skip = 0,
            int take = 0,
            Expression<Func<TValue, bool>>? where = null,
            Expression<Func<TValue, TOrderKey>>? orderBy = null,
            bool ascending = true)
        {
            IQueryable<TValue> query = DbSet;
            if (where != null)
            {
                query = query.Where(where);
            }

            if (orderBy != null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return query;
        }
    }
}
