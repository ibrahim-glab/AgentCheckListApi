

// create IRegisterationService Interface
using AgentCheckListApi.Model;
using AgentCheckListApi.Helper;

namespace AgentCheckListApi.Interfaces
{
    public interface IRegisterationService
    {
        public ServiceResult RegisterUser(string id,User user);
     
        public ServiceResult UpdateUser(string id,User user);
        public ServiceResult DeleteUser(string id , string organizationId);
        public ServiceResult GetUserByOrganization(string id , string organizationId);
        public List<User> GetUsers();
        public bool RegisterOrganization(Organization organization);
        public bool UpdateOrganization(Organization organization);
        public bool DeleteOrganization(string id);
        public Organization GetOrganizationById(string id);
        public List<Organization> GetOrganizations();
    }
}