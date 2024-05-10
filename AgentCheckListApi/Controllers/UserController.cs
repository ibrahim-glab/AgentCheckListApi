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
using Microsoft.AspNetCore.Authorization;

namespace AgentCheckListApi.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Add Mongo Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IAuthService _authService ;
        private readonly IInspectionService _inspectionService;
        private readonly ILogger<UserController> _logger;
        private readonly IRegisterationService _registerationService;
        public UserController(ILogger<UserController> logger, MongoDbService<Organization> mongoDbService, IRegisterationService regestirationService , IAuthService authService ,  IInspectionService inspectionService)
        {
            _logger = logger;
            _mongoDbService = mongoDbService;
            _registerationService = regestirationService;
            _authService = authService;
            _inspectionService = inspectionService;
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
        [AllowAnonymous]
        [HttpPost(Name ="Login")]
        public IActionResult Login  ([FromBody] UserLoginDto user){
            ServiceResult serviceResult = _authService.Authnticate(user.UserName , user.Password);
            if(!serviceResult.Success)
                return Unauthorized(serviceResult);
            return Ok(new {token =serviceResult.Data });
        }
        // api/User/{id}/Form
        [Authorize]
        [HttpGet("{id}/Forms")]
        public IActionResult Get(string id){
            if(id is null)
                return BadRequest();
            var ServiceResult = _inspectionService.GetFormsByUserId(id);
            return Ok(ServiceResult.Data);
        }

    }
}