using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Services
{
    /// <summary>
    /// Interface for service that will handle business logic related to the Entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityService<T> where T : EntityBase
    {
        IEnumerable<T> GetItems();
        T GetItem(int id);
        void AddItem(T item);
        void RemoveItem(T item);
        IEnumerable<T> GetItems(string locationName);
    }
}
