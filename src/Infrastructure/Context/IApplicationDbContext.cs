using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Context
{
    /// <summary>
    /// Resposnible for populating entities from an in memory 'database'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApplicationDbContext<T> where T : EntityBase 
    {
        /// <summary>
        /// The in memory 'database' to use for this prototype.
        /// </summary>
        IList<T> Entities { get; }
    }
}