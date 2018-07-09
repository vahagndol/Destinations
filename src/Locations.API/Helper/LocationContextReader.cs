using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Services;
using Newtonsoft.Json;

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
                    locations = JsonConvert.DeserializeObject<List<Location>>(json);
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
