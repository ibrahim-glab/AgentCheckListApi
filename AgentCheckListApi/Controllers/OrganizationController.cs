// Create organization Controller Web Api 
//add using statements

using AgentCheckListApi.Helper;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
            var filterid = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = collection.Find(filterid);
            return Ok(list);
        }

        //Post : api/Organization
        [HttpPost]
        public IActionResult Post([FromBody] Organization organization)
        {

            // query first if there is a record with same LicenseId 

            if(!ModelState.IsValid)     
                return BadRequest();
            
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

            if(!ModelState.IsValid)     
                return BadRequest();
            
            // add Validation
            if (organization == null)
            {
                return BadRequest();
            }
            // check if there is a record with same id
            var filterid = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = collection.Find(filterid).ToList();
            if (list.Count == 0)
                return NotFound("Organization not found");
            try
            {


                var updateResult = collection.FindOneAndUpdate<Organization>(Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Organization>.Update.Set("LicenseId", organization.LicenseId)
                .Set("BankAccount", organization.BankAccount)
                .Set("FinancialLimitFrom", organization.FinancialLimitFrom)
                .Set("FinancialLimitTo", organization.FinancialLimitTo)
                .Set("Organization_name", organization.Organization_name)
                .Set("Organization_status", organization.Organization_status)
                .Set("OrganizationFinancialId", organization.OrganizationFinancialId)
                .Set("OrganizationType", organization.OrganizationType) , new FindOneAndUpdateOptions<Organization, Organization>() { ReturnDocument = ReturnDocument.After }
                );
               if (updateResult == null)
                return NotFound("Organization not found");
                return Ok(updateResult);

            }
            catch (System.Exception ex)
            {
                 // TODO
                 return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
          
        }


        // api/Organization/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {

            try
            {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var filter = Builders<Organization>.Filter.Eq("_Id", id);

            var deleteResult = collection.DeleteOne(filter);
            if (deleteResult.DeletedCount == 0)
                return NotFound("Organization not found");
            return Ok();
            }
            catch (System.Exception ex)
            {
                 // TODO
                 return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        // api/Organization/{id}    
        // [HttpPatch("{id}")]
        // public IActionResult Patch(string id)
        // {
        //     // add Validation 
        //     if (id == null)
        //     {
        //         return BadRequest();
        //     }

        //     var collection = _mongoDbService.GetCollection<Organization>("organizations");


        //     var filter = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
        //     var updateResult = collection.FindOneAndUpdate<Organization>(filter, Builders<Organization>.Update.Set("Organization_status", true), new FindOneAndUpdateOptions<Organization, Organization>() { ReturnDocument = ReturnDocument.After });
        //     if (updateResult == null)
        //         return NotFound("Organization not found");
        //     return Ok(updateResult);         
        //     //return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        // }
      

    }
}

