// Create organization Controller Web Api 
//add using statements

using AgentCheckListApi.Helper;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
namespace AgentCheckListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly MongoDbService<Organization> _mongoDbService;
        public OrganizationController(ILogger<OrganizationController> logger, MongoDbService<Organization> mongoDbService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
        }


        //Get: api/Organization All
        [HttpGet]
        public IActionResult Get()
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var list = collection.Find(FilterDefinition<Organization>.Empty).ToList();
            return Ok(list);
        }

        //Get   : api/Organization/{id}
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var filter = Builders<Organization>.Filter.Eq("_Id", id);
            var list = collection.Find(filter).ToList();
            return Ok(list);
        }

        //Post : api/Organization
        [HttpPost]
        public IActionResult Post([FromBody] Organization organization)
        {

            // query first if there is a record with same LicenseId 


            // add Validation
            if (organization == null)
            {
                return BadRequest();
            }
            try
            {
                var collection = _mongoDbService.GetCollection<Organization>("organizations");
                // check if there is a record with same LicenseId
                var filter = Builders<Organization>.Filter.Eq("LicenseId", organization.LicenseId);
                var list = collection.Find(filter).ToList();
                if (list.Count > 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "LicenseId already exists");

                collection.InsertOne(organization);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        //Put : api/Organization/{id} Mongo 
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Organization organization)
        {
         var collection = _mongoDbService.GetCollection<Organization>("organizations");


            // add Validation
            if (organization == null)
            {
                return BadRequest();
            }
            // check if there is a record with same id
            var filterid = Builders<Organization>.Filter.Eq("_Id", id);
            var list = collection.Find(filterid).ToList();
            if (list.Count == 0)
                return NotFound("Organization not found");
            
            
            var update = Builders<Organization>.Update
                .Set("LicenseId", organization.LicenseId)
                .Set("BankAccount", organization.BankAccount)
                .Set("FinancialLimitFrom", organization.FinancialLimitFrom)
                .Set("FinancialLimitTo", organization.FinancialLimitTo)
                .Set("Organization_name", organization.Organization_name)
                .Set("Organization_status", organization.Organization_status)
                .Set("OrganizationFinancialId", organization.OrganizationFinancialId)
                .Set("OrganizationType", organization.OrganizationType);
               
            collection.UpdateOne(filterid, update);
            return Ok();
        }
      

    }
}

