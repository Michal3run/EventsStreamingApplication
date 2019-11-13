using System.Collections.Generic;
using EventsWebDashboard.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebDashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsManager _eventsManager;

        public EventsController(IEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<TopicModel>> Get()
        {
            var eventTopics = _eventsManager.GetBestEventTopics(10);
            return eventTopics;
        }

        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    var x = _eventsManager.GetBestEventTopics(10);

        //    return new string[] { "value1", "value2" };
        //}

        

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}")]
        public ActionResult<string> CleanEvents()
        {
            var cleanResult = _eventsManager.CleanEvents();
            return cleanResult;
        }
    }
}
