// Create Implementation of IInspectionService
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using AgentCheckListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
namespace AgentCheckListApi.Services
{   
    public class InspectionService : IInspectionService
    {
        private readonly IMongoCollection<CheckList> _CheckList;
        private readonly IMongoQueryable<CheckList> _CheckListQueryable;
        private readonly IMongoCollection<Form> _FormCollection;
        private readonly IMongoQueryable<Form> _FormQueryable;
        private readonly IRegisterationService _registerationService;


        public InspectionService(IOptions<MongoDb> configuration , IRegisterationService registerationService)
        {
            var client = new MongoClient(configuration.Value.ConnectionString);
            var database = client.GetDatabase(configuration.Value.Database);
            _CheckList = database.GetCollection<CheckList>("checklists");
            _FormCollection = database.GetCollection<Form>("forms");
            _CheckListQueryable = _CheckList.AsQueryable();
            _FormQueryable = _FormCollection.AsQueryable();
            _registerationService = registerationService  ;
        }

        public ServiceResult CreateChecklist(CheckList checklist)
        {
            var organization = _registerationService.GetOrganizationById(checklist.OrganizationId);
            if (organization is null)
                return new ServiceResult { Success = false, Message = "Organization not found" };
                try
                {
                    _CheckList.InsertOne(checklist);
                        return new ServiceResult { Success = true, Message = "CheckList Created", Data = checklist };
                }
                catch (System.Exception ex)
                {
                    return new ServiceResult { Success = false, Message = ex.Message };
                }
        }
        // get CheckList by id 
        public CheckList GetCheckListById(string id){
            return _CheckListQueryable.FirstOrDefault(x => x.Id == id) ;
        }

        public List<CheckList> GetCheckLists()
        {
             return _CheckListQueryable.ToList();
        }

        public List<CheckList> GetCheckListsByOrganizationId(string organizationId)
        {
            // using mongo collection 
            var checkLis = _CheckList.Find(Builders<CheckList>.Filter.Eq("organizationId", ObjectId.Parse(organizationId))).ToList();
            return _CheckListQueryable.Where(x => x.OrganizationId == organizationId).ToList();
        }

        public ServiceResult UpdateCheckList(string id ,CheckList checklist)
        {
            // if not Exists return not found
            if (GetCheckListById(id) is null)
                return new ServiceResult { Success = false, Message = "CheckList not found" };
            try
            {    
            _CheckList.ReplaceOne(x => x.Id == checklist.Id, checklist);
            return new ServiceResult { Success = true, Message = "CheckList Updated", Data = checklist };
            }
            catch (System.Exception ex)
            {
                return new ServiceResult { Success = false, Message = ex.Message };
            }
        }
        public ServiceResult DeleteCheckList(string id)
        {
            // if not Exists return not found
            if (GetCheckListById(id) is null)
                return new ServiceResult { Success = false, Message = "CheckList not found" };
            try
            {
                _CheckList.DeleteOne(x => x.Id == id);
                return new ServiceResult { Success = true, Message = "CheckList Deleted" };
            }
            catch (System.Exception ex)
            {
                return new ServiceResult { Success = false, Message = ex.Message };
            }
    }

        public ServiceResult SumbitForm(string id, Form form)
        {
            if (GetCheckListById(id) is null)
                return new ServiceResult { Success = false, Message = "CheckList not found" };
            try
            {
                _FormCollection.InsertOne(form);
                return new ServiceResult { Success = true, Message = "Form Sumbited", Data = form };
            }
            catch (System.Exception ex)
            {
                return new ServiceResult { Success = false, Message = ex.Message };
            }           
        }
        // Get Forms By CheckListId
        public ServiceResult GetFormsByCheckListId(string id){
              if (GetCheckListById(id) is null)
                return new ServiceResult { Success = false, Message = "CheckList not found" };
            var forms = _FormQueryable.Where(x => x.ChecklistID == id).ToList();
            return new ServiceResult { Success = true, Message = "Forms Found", Data = forms };
        }
    }
}