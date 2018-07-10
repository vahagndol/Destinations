using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    public class PlacesController : Controller
    {
        private readonly HttpClient _client;

        public PlacesController(IHttpClientFactory httpClientFactory)
        {
            var factory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _client = factory.CreateClient("places");
        }

        [HttpGet]
        [Route("GetAll/{locationId}")]
        public async Task<ActionResult<string>> GetAll(string locationId)
        {
            var response = await _client.GetAsync($"/api/place/{locationId}");
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return result;
        }
    }
}
