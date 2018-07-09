using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Locations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IEntityService<Location> _locationService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IEntityService<Location> locationService, ILogger<LocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }

        // GET api/location
        [HttpGet]
        public ActionResult<IEnumerable<Location>> Get()
        {
            return Ok(_locationService.GetItems());
        }

        // GET api/values
        [HttpGet("{id}")]
        public ActionResult<Location> Get(int id)
        {
            var location = _locationService.GetItem(id);
            if (location != null)
            {
                return Ok(location);
            }
            var message = $"Location with id={id} not found.";
            _logger.LogWarning(message);
            return NotFound(message);
        }
    }
}