using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{

    /// <summary>
    /// Service to handle business logic related to Entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Infrastructure.Services.IEntityService{T}" />
    public class EntityService<T> : IEntityService<T> where T : EntityBase
    {
        private readonly IRepository<T> _repository;

        public EntityService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> GetItems()
        {
            return _repository.GetAll();
        }

        public T GetItem(int id)
        {
            return _repository.FindById(id);
        }

        public void AddItem(T item)
        {
            _repository.Insert(item);
        }

        public void RemoveItem(T item)
        {
            _repository.Delete(item);
        }

        public virtual IEnumerable<T> GetItems(string locationName)
        {
            var all = _repository.GetAll();
            return all.Where(entity => entity.Name == locationName).ToList();
        }
    }
}
