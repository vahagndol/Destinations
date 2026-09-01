using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.Context;

namespace Places.API.Helper
{
    public class PlaceContextReader<T> : IContextReader<T> where T : Place
    {
        public IList<T> ReadContext()
        {
            try
            {
                IEnumerable<PlacesDto> placesDtos;
                using (var r = new StreamReader("places.json"))
                {
                    var json = r.ReadToEnd();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    placesDtos = JsonSerializer.Deserialize<List<PlacesDto>>(json, options) ?? new List<PlacesDto>();
                }

                var places = new List<Place>();
                foreach (var placeDto in placesDtos)
                {
                    foreach (var place in placeDto.Places)
                    {
                        place.CityId = placeDto.CityId;                        
                    }
                    places.AddRange(placeDto.Places);
                }

                return (IList<T>)places;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
