using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Places.API.Helper
{
    public class PlaceService : EntityService<Place>
    {
        public PlaceService(IRepository<Place> repository) :
            base(repository)
        {
            
        }

        public override IEnumerable<Place> GetItems(string cityId)
        {
            if (!int.TryParse(cityId, out var id))
                return null;

            var all = GetItems();
            return all.Where(entity => entity.CityId == id).ToList();
        }
    }
}
