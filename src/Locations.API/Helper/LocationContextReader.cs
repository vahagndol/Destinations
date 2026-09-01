using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Services;

namespace Locations.API.Helper
{
    public class LocationContextReader<T> : IContextReader<T> where T : Location
    {
        public IList<T> ReadContext()
        {
            try
            {
                IList<Location> locations;
                using (var r = new StreamReader("locations.json"))
                {
                    var json = r.ReadToEnd();
                    locations = JsonSerializer.Deserialize<List<Location>>(json) ?? new List<Location>();
                }

                return (IList<T>) locations;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
