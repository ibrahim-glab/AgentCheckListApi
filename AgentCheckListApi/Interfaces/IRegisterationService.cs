

// create IRegisterationService Interface
using AgentCheckListApi.Model;
using AgentCheckListApi.Helper;

namespace AgentCheckListApi.Interfaces
{
    public interface IRegisterationService
    {
        public ServiceResult RegisterUser(User user , Permission permission);
     
        public ServiceResult UpdateUser(string id,User user);
        public ServiceResult DeleteUser(string id , string organizationId);
        public ServiceResult GetUserByOrganization(string id , string organizationId);
        public ServiceResult RegisterOrganizationAdmin( string SuperAdminId, User user , Permission permission);
        public List<User> GetUsers();
        public bool RegisterOrganization(Organization organization);

        public bool UpdateOrganization(Organization organization);
        public bool DeleteOrganization(string id);
        public Organization GetOrganizationById(string id);
        public ServiceResult GetByUserId(string id);
        //GetPermissionById
        public Permission GetPermissionById(string id);
        // Create Permission
        public ServiceResult CreatePermission( Permission permission);
        public List<Organization> GetOrganizations();
        // add Get permission function
        public  List<Permission> GetPermissions();
    }
}