// User Controller Api EndPoint 

// add using statements

using AgentCheckListApi.Helper;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgentCheckListApi.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Add Mongo Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger , MongoDbService<Organization> mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
           var collection = _mongoDbService.GetCollection<Organization>("organizations");
           var list = collection.Find(FilterDefinition<Organization>.Empty).ToList();
            return Ok(list);
        }

        //Post : api/Organization   
        [HttpPost]
        public IActionResult Post([FromBody] Organization organization)
        {   
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            collection.InsertOne(organization);
            return Ok(organization);
        }
    }
}