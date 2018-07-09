using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Interface for Repository Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Finds the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T FindById(int id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert(T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);
    }
}
