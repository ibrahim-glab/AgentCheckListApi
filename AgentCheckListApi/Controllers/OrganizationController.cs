// Create organization Controller Web Api 
//add using statements

using System.Security.Claims;
using AgentCheckListApi.Enums;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
namespace AgentCheckListApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IRegisterationService _registerationService;
        private readonly IInspectionService _inspectionService;
        public OrganizationController(IRegisterationService regestirationService, ILogger<OrganizationController> logger, MongoDbService<Organization> mongoDbService, IInspectionService inspectionService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _registerationService = regestirationService;
            _inspectionService = inspectionService;
        }


        //Get: api/Organization All
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult Get()
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var list = collection.Find<Organization>(FilterDefinition<Organization>.Empty).ToList();
            return Ok(list);
        }

        //Get   : api/Organization/{id}
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var filterid = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = collection.Find(filterid).FirstOrDefault();
            return Ok(list);
        }

        //Post : api/Organization
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public IActionResult Post([FromBody] Organization organization)
        {

            // query first if there is a record with same LicenseId 

            if (!ModelState.IsValid)
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
        [Authorize(Roles = "SuperAdmin")]

        //Put : api/Organization/{id} Mongo 
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Organization organization)
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");

            if (!ModelState.IsValid)
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
                .Set("OrganizationType", organization.OrganizationType), new FindOneAndUpdateOptions<Organization, Organization>() { ReturnDocument = ReturnDocument.After }
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

        [Authorize(Roles = "SuperAdmin")]
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
        // Post: api/Organization/{id}/Users
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]

        [HttpPost("{id}/Users")]
        public IActionResult Post(string id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                Permission permission = new Permission
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    IsActive = user.IsActive,
                    UserMobileNumber = user.UserMobileNumber,
                    Role = UserRole.AgentField
                };
                user.OrganizationId = id;
                user.OrgAdminId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var ServiceResult = _registerationService.RegisterUser(user, permission);
                if (!ServiceResult.Success)
                    return NotFound(ServiceResult.Message);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        //get:api/Organization/{id}/Users/{userId}
        [HttpGet("{id}/Users/{userId}")]
        public IActionResult Get(string id, string userId)
        {
            var ServiceResult = _registerationService.GetUserByOrganization(userId, id);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);
            return Ok(ServiceResult.Data);
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        // api/Organization/{id}/Users/{userId}
        [HttpDelete("{id}/Users/{userId}")]
        public IActionResult Delete(string id, string userId)
        {
            var ServiceResult = _registerationService.DeleteUser(userId, id);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);

            return Ok(ServiceResult.Data);
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        // api/Organization/{id}/Users/{userId}
        [HttpPut("{id}/Users/{userId}")]
        public IActionResult Put(string id, string userId, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var organization = _registerationService.GetOrganizationById(id);
            if (organization is null)
                return NotFound("Organization not found");
            user.OrganizationId = id;
            user.OrgAdminId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var ServiceResult = _registerationService.UpdateUser(userId, user);

            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);

            return Ok(ServiceResult.Data);
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        //api: Organization/{id}/Users
        [HttpGet("{id}/Users")]
        public IActionResult GetOrgnizationUsers(string id)
        {
            var collection = _mongoDbService.GetCollection<Organization>("organizations");
            var filterid = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = collection.Find(filterid).ToList();
            if (list.Count == 0)
                return NotFound("Organization not found");
            var collectionUser = _mongoDbService.GetCollection<User>("users");
            //get all users from organization where OrganizationId = id
            var filter = Builders<User>.Filter.Eq("OrganizationId", ObjectId.Parse(id));
            var listUser = collectionUser.Find(filter).ToList();
            return Ok(listUser);
        }
        // api/Organization/{id}/CheckLists
        [HttpGet("{id}/CheckLists")]
        public IActionResult GetOrgnizationCheckLists(string id)
        {
            var listCheckList = _inspectionService.GetCheckListsByOrganizationId(id);
            return Ok(listCheckList);
        }
        [HttpPost("{id}/OrganizationAdmin")]
        public IActionResult PostOrganizationAdmin(string id, [FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();
            if (id is null)
                return BadRequest();
            Permission permission1 = new Permission
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IsActive = user.IsActive,
                UserMobileNumber = user.UserMobileNumber,
                Role = UserRole.OrgAdmin,
            };
            var userid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userid is null)
                return BadRequest();
            ServiceResult serviceResult = _registerationService.RegisterOrganizationAdmin(user, permission1, userid.Value);
            if (!serviceResult.Success)
                return BadRequest(serviceResult.Message);
            return Ok(user);
        }
        // Put: api/User/{id}/OrganizationAdmin
        [HttpPut("{id}/OrganizationAdmin")]
        public IActionResult PutOrganizationAdmin(string id, [FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest();
                if (!ModelState.IsValid)
                    return BadRequest();
                //new Permission{Id = ObjectId.GenerateNewId().ToString(), UserMobileNumber = user.UserMobileNumber, Role = UserRole.OrgAdmin , IsActive = user.IsActive}

                ServiceResult serviceResult = _registerationService.UpdateOrganizationAdmin(user, new Permission { Id = ObjectId.GenerateNewId().ToString(), IsActive = user.IsActive, UserMobileNumber = user.UserMobileNumber, Role = UserRole.OrgAdmin });

                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}

