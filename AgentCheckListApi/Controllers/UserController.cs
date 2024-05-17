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
    [Authorize(Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Add Mongo Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IAuthService _authService;
        private readonly IInspectionService _inspectionService;
        private readonly ILogger<UserController> _logger;
        private readonly IRegisterationService _registerationService;
        public UserController(ILogger<UserController> logger, MongoDbService<Organization> mongoDbService, IRegisterationService regestirationService, IAuthService authService, IInspectionService inspectionService)
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


        [AllowAnonymous]
        [HttpPost(Name = "Login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            ServiceResult serviceResult = _authService.Authnticate(user.UserName, user.Password);
            if (!serviceResult.Success)
                return Unauthorized(serviceResult);
            return Ok(new { token = serviceResult.Data });
        }
        // api/User/{id}/Form
        [Authorize]
        [HttpGet("{id}/Forms")]
        public IActionResult Get(string id)
        {
            if (id is null)
                return BadRequest();
            var ServiceResult = _inspectionService.GetFormsByUserId(id);
            return Ok(ServiceResult.Data);
        }


    }
}