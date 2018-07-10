using System.Collections.Generic;
using Domain.Entities;

namespace Places.API.Helper
{
    public class PlacesDto 
    {
        public int CityId { get; set; }

        public IEnumerable<Place> Places { get; set; } = new List<Place>();

        public PlacesDto(int cityId)
        {
            CityId = cityId;
        }
    }
}
