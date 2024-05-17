using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgentCheckListApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientsService clientsService;

        public ClientController(IClientsService _clientsService)
        {
            clientsService = _clientsService;
        }

        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        [HttpPost]
        public IActionResult Post([FromBody] AgentCheckListApi.Model.Directory directory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (directory is null)
                return BadRequest(ModelState);
            ServiceResult serivceResult = clientsService.Create(directory);
            if (!serivceResult.Success)
                return BadRequest(serivceResult.Message);
            return Ok(serivceResult.Data);

        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(clientsService.GetDirectories());
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (id is null)
                return BadRequest();
            var serviceResult = clientsService.Get(id);
            if (!serviceResult.Success)
                return NotFound(serviceResult.Message);
            return Ok(clientsService.Get(id));
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] AgentCheckListApi.Model.Directory directory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (directory is null)
                return BadRequest(ModelState);
            ServiceResult serivceResult = clientsService.Update(directory);
            if (!serivceResult.Success)
                return BadRequest(serivceResult.Message);
            return Ok(serivceResult.Data);
        }
        [Authorize(Roles = "SuperAdmin,OrgAdmin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var ServiceResult = clientsService.Delete(id);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);
            return Ok();
        }

        //Get Directories for Agent 
        [Authorize(Roles = "AgentField")]
        [HttpGet("DirectoriesForAgent")]
        public IActionResult GetDirectories()
        {
            // Get Id of User From JWT Token sender of request 
            var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (id is null)
                return BadRequest();
            var serivceResult = clientsService.GetDirectoriesForUser(id.Value);
            if (!serivceResult.Success)
                return BadRequest(serivceResult.Message);
            return Ok(serivceResult.Data);
        }
    }
}