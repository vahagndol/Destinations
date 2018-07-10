using System.Collections.Generic;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Places.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IEntityService<Place> _placeService;
        private readonly ILogger<PlaceController> _logger;

        public PlaceController(IEntityService<Place> placeService, ILogger<PlaceController> logger)
        {
            _placeService = placeService;
            _logger = logger;
        }

        // GET api/place
        [HttpGet]
        public ActionResult<IEnumerable<Place>> Get()
        {
            return Ok(_placeService.GetItems());
        }

        // GET api/place
        [HttpGet("{placeId}")]
        public ActionResult<IEnumerable<Place>> Get(string placeId)
        {
            return Ok(_placeService.GetItems(placeId));
        }

       
    }
}