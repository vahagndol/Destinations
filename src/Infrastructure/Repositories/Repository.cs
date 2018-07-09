using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of Repository Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Infrastructure.Repositories.IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly IApplicationDbContext<T> _dbContext;

        public Repository(IApplicationDbContext<T> dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return  _dbContext.Entities;
        }

        public T FindById(int id)
        {
            return _dbContext.Entities.FirstOrDefault(i => i.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var item = FindById(entity.Id);
            if ( item!=null)
            {
                throw new ArgumentException($"Entity with the id={entity.Id} already exist in the repository.");
            }

            _dbContext.Entities.Add(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _dbContext.Entities.Remove(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var item = FindById(entity.Id);
            if (item == null)
            {
                throw new ArgumentException($"Entity with the id={entity.Id} doesn't exist in the repository.");
            }
        }
    }
}
