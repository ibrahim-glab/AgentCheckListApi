
using AgentCheckListApi.Helper;
using AgentCheckListApi.Model;

namespace AgentCheckListApi.Interfaces
{
    public interface IClientsService
    {
        public ServiceResult Create(AgentCheckListApi.Model.Directory directory);
        public ServiceResult Get(string id);
        public ServiceResult Update(AgentCheckListApi.Model.Directory directory);
        public ServiceResult Delete(string id);
        //Get ALl Directory
        public ServiceResult GetDirectories();

    }
}