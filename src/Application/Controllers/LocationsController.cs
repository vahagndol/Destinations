using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private readonly HttpClient _client;

        public LocationsController(IHttpClientFactory httpClientFactory)
        {
            var factory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _client = factory.CreateClient("locations");
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<string>> GetAll()
        {
            var response = await _client.GetAsync($"/api/location");
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return result;
        }
    }
}
