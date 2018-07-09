using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Context
{
    /// <summary>
    /// Reads entites from a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContextReader<T> where T : EntityBase
    {
        /// <summary>
        /// Reads the context.
        /// </summary>
        /// <returns></returns>
        IList<T> ReadContext();
    }
}
