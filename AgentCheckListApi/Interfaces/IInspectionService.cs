// Create Inspection Service 
// add uisng 

using AgentCheckListApi.Model;
using AgentCheckListApi.Helper;

namespace AgentCheckListApi.Interfaces
{
    public interface IInspectionService
    {
        public ServiceResult CreateChecklist(CheckList checklist);
        // Get CheckList By Id
        public CheckList GetCheckListById(string id);
        public ServiceResult UpdateCheckList(string id ,  CheckList checklist);
        public ServiceResult DeleteCheckList(string id);
        public List<CheckList> GetCheckLists();
        public List<CheckList> GetCheckListsByOrganizationId(string organizationId);

        public ServiceResult SumbitForm(string id , Form form);
        public ServiceResult GetFormsByCheckListId(string id);
        public ServiceResult GetFormsByUserId(string userId);
    }
}