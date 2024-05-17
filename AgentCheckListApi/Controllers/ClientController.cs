using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}