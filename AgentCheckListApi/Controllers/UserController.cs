// User Controller Api EndPoint 

// add using statements

using System.Runtime.CompilerServices;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using AgentCheckListApi.Enums;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgentCheckListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Add Mongo Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly ILogger<UserController> _logger;
        private readonly IRegisterationService _registerationService;
        public UserController(ILogger<UserController> logger, MongoDbService<Organization> mongoDbService, IRegisterationService regestirationService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _registerationService = regestirationService;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult Get()
        {
            var collection = _mongoDbService.GetCollection<User>("users");
            var list = collection.Find(FilterDefinition<User>.Empty).ToList();
            return Ok(list);
        }

        // in this method we will add a new user to the database who is Organization Admin

        [HttpPost("{id}/OrganizationAdmin")]
        public IActionResult PostOrganizationAdmin(string id, [FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)        
                return BadRequest();
            if(id is null)
                return BadRequest();
          Permission permission1 = new Permission
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IsActive= user.IsActive,
                UserMobileNumber = user.UserMobileNumber,
                Role = UserRole.OrgAdmin,
            };
            ServiceResult serviceResult = _registerationService.RegisterOrganizationAdmin( user, permission1 ,id);
            if (!serviceResult.Success)
                return BadRequest(serviceResult.Message);
            return Ok(user);
        }
        // Put: api/User/{id}/OrganizationAdmin

        [HttpPut("{id}/OrganizationAdmin")]
        public IActionResult PutOrganizationAdmin(string id, [FromBody] User user)
        {
            try{
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();
                //new Permission{Id = ObjectId.GenerateNewId().ToString(), UserMobileNumber = user.UserMobileNumber, Role = UserRole.OrgAdmin , IsActive = user.IsActive}

            ServiceResult serviceResult = _registerationService.UpdateOrganizationAdmin(user, new Permission { Id = ObjectId.GenerateNewId().ToString(), IsActive = user.IsActive, UserMobileNumber = user.UserMobileNumber, Role = UserRole.OrgAdmin });

            return Ok(serviceResult);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

    }
}