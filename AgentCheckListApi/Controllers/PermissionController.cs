
// Create Permission Controller To manage Crud operation in Permission Model 

// add using statements
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
namespace AgentCheckListApi.Controllers
{
    [Authorize(Roles = "SuperAdmin,OrgAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {   
        private readonly IRegisterationService _registerationService;
        public PermissionController( IRegisterationService registerationService)
        {
            _registerationService = registerationService;
        }
        //Get: api/Permission All
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_registerationService.GetPermissions());
        }
        //Get   : api/Permission/{id}
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var permission = _registerationService.GetPermissionById(id);
            if (permission is null)
                return NotFound();
            return Ok(permission);
        }   
    }
}