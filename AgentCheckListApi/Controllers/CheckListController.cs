// Create Endpoints for CheckList Model 

using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
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
    public class CheckListController : ControllerBase
    {
        private readonly IInspectionService _inspectionService;

        public CheckListController(IInspectionService inspectionService)
        {
            _inspectionService = inspectionService;
        }
        [HttpGet]  
        public IActionResult Get()
        {
            return Ok(_inspectionService.GetCheckLists());
        }
        // POST : api/CheckList
        [HttpPost]
        public IActionResult Post([FromBody] CheckList checklist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ServiceResult = _inspectionService.CreateChecklist(checklist);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);
            return Ok(ServiceResult.Data);
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var checklist = _inspectionService.GetCheckListById(id);
            if (checklist is null)
                return NotFound();
            return Ok(checklist);
        }
        // PUT : api/CheckList/{id}
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] CheckList checklist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != checklist.Id)
                return BadRequest();
            var ServiceResult = _inspectionService.UpdateCheckList(id,checklist);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);
            return Ok(ServiceResult.Data);
        }

        // DELETE : api/CheckList/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var ServiceResult = _inspectionService.DeleteCheckList(id);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);
            return Ok(ServiceResult);
        }

        // Post : api/CheckList/{id}/Form

        // Post : api/CheckList/{id}/Form
        [HttpPost("{id}/Form")]
        public IActionResult PostForm(string id, [FromBody] Form form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ServiceResult = _inspectionService.SumbitForm(id, form);
            if (!ServiceResult.Success)
                return NotFound(ServiceResult.Message);  
            return Ok(ServiceResult);
        }
        // Get : api/CheckList/{id}/Form

        // Get : api/CheckList/{id}/Form
        [HttpGet("{id}/Form")]
        public IActionResult GetAll(string id)
        {
            var result = _inspectionService.GetFormsByCheckListId(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }



    }
}