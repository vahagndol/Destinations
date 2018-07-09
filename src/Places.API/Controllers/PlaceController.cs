using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Places.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly ILogger<PlaceController> _logger;

        public PlaceController(ILogger<PlaceController> logger)
        {
            _logger = logger;
        }

        // GET api/place
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}