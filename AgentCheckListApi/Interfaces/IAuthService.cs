// create Auth Service Interface 
//add using 
using AgentCheckListApi.Helper;
using AgentCheckListApi.Model;

namespace AgentCheckListApi.Interfaces
{
    public interface IAuthService
    {
        public ServiceResult Authnticate(string userName, string password);
    }

}