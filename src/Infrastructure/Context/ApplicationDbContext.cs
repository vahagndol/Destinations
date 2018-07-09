using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Context
{
    /// <summary>
    /// ApplicationDbContext implements interface IApplicationDbContext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Infrastructure.Context.IApplicationDbContext{T}" />
    public class ApplicationDbContext<T> : IApplicationDbContext<T> where T: EntityBase
    {
        public IList<T> Entities { get; }

        public ApplicationDbContext(IContextReader<T> reader)
        {
            Entities = reader.ReadContext();
        }        
    }
}
