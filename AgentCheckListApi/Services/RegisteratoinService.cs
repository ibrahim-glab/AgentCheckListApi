// Implement IRegisteratoinService
// c:/Users/Bebo/source/repos/AgentCheckListApi/AgentCheckListApi/Services/RegisteratoinService.cs
using AgentCheckListApi.Enums;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AgentCheckListApi.Services
{
    public class RegisteratoinService : IRegisterationService
    {
        //replace With MongoDb Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoQueryable<User> _usersquery;
        private readonly IMongoQueryable<Permission> _Permissionquery;
        private readonly IMongoCollection<Organization> _organizations;
        // add mongo collection Permissions
        private readonly IMongoCollection<Permission> _permissions;
        public RegisteratoinService(MongoDbService<Organization> mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _users = _mongoDbService.GetCollection<User>("users");
            _usersquery = _users.AsQueryable();
            _organizations = _mongoDbService.GetCollection<Organization>("organizations");
            _permissions = _mongoDbService.GetCollection<Permission>("permissions");
            _Permissionquery = _permissions.AsQueryable();
        }
        public User Register(User user)
        {
            _users.InsertOne(user);
            return user;
        }
        public Organization Register(Organization organization)
        {
            _organizations.InsertOne(organization);
            return organization;
        }
        public User Authenticate(string username, string password)
        {
            var filter = Builders<User>.Filter.Eq("Username", username) & Builders<User>.Filter.Eq("Password", password);
            var user = _users.Find(filter).FirstOrDefault();
            return user;
        }
        public ServiceResult GetByUserId(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
            var user = _users.Find(filter).FirstOrDefault();
            if (user is null)
                return new ServiceResult { Success = false, Message = "User Not Found" };
            return new ServiceResult { Success = true, Message = "User Found", Data = user };
        }
        //GetPermissionById
        public Permission GetPermissionById(string id)
        {
            var filter = Builders<Permission>.Filter.Eq("_id", ObjectId.Parse(id));
            var permission = _permissions.Find(filter).FirstOrDefault();
            return permission;
        }
        // Create permission
        public ServiceResult CreatePermission(Permission permission)
        {
            // Chceck if permission already exists with  the same mobile number
            var check = _Permissionquery.FirstOrDefault(x => x.UserMobileNumber == permission.UserMobileNumber);
             if (check is not null)
                return new ServiceResult { Success = false, Message = "Permission With Same Mobile Number Already Exists" };
            _permissions.InsertOne(permission);
            return new ServiceResult { Success = true, Message = "Permission Created", Data = permission };
        }

        public Organization GetOrganizationById(string id)
        {
            var filter = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var organization = _organizations.Find(filter).FirstOrDefault();
            return organization;
        }

        public ServiceResult RegisterUser(User user, Permission permission)
        {

            if (!IsUserAlreadyRegistered(user.UserMobileNumber, user.UserNationalId))
            {
                if (IsOrganizationAlreadyRegistered(user.OrganizationId))
                {
                    // user.OrganizationId = ObjectId.Parse(id); //user.OrganizationId = id;
                    var res = CreatePermission(permission);
                    if (!res.Success)
                        return res;
                    var QueryableUser = _usersquery.FirstOrDefault(x=> x.OrganizationId == user.OrganizationId);
                    user.OrgAdminId = QueryableUser?.OrgAdminId ?? string.Empty;
                   // user.OrgAdminId = _users.Find(Builders<User>.Filter.Eq("organizationId", ObjectId.Parse(user.OrganizationId))).FirstOrDefault()?.OrgAdminId ?? string.Empty;
                    
                    _users.InsertOne(user);
                    return new ServiceResult { Success = true, Message = "User Registered", Data = user };
                }
                return new ServiceResult { Success = false, Message = "Organization Not Found" };
            }
            return new ServiceResult { Success = false, Message = "User Already Registered with this Phone Number or National Id" };

        }
        // RegisterOrganizationAdmin
        public ServiceResult RegisterOrganizationAdmin( User user, Permission permission ,string SuperAdminId = "")
        {
           
          user.Id = ObjectId.GenerateNewId().ToString();
          user.OrgAdminId = user.Id;
          var serviceResult1 = RegisterUser(user, permission);
            if (!serviceResult1.Success)
                return serviceResult1;
            return new ServiceResult { Success = true, Message = "User Created", Data = user };

        }   
        //Update Organization admin
        public ServiceResult UpdateOrganizationAdmin(User user , Permission permission)
        {
         var olduser = _usersquery.FirstOrDefault(x => x.OrgAdminId != null && x.OrganizationId == user.OrganizationId);
         if(olduser is null){
            var res = RegisterOrganizationAdmin( user, permission);
            if (!res.Success)
                return res;
            return new ServiceResult { Success = true, Message = "User Created", Data = user };
         }

         var ResultForDeletePermission = _permissions.FindOneAndDelete(Builders<Permission>.Filter.Eq("UserMobileNumber", olduser.UserMobileNumber));
            user.Id = ObjectId.GenerateNewId().ToString();
            user.OrgAdminId = user.Id;
            user =  _users.FindOneAndReplace(Builders<User>.Filter.Eq("_id", ObjectId.Parse(user.Id)), user , new FindOneAndReplaceOptions<User> {IsUpsert = true , ReturnDocument = ReturnDocument.After});
            var res2 = _permissions.FindOneAndReplace(Builders<Permission>.Filter.Eq("UserMobileNumber  ", user.UserMobileNumber), permission, new FindOneAndReplaceOptions<Permission> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
            var ResultForUpdateMany = _users.UpdateMany(Builders<User>.Filter.Eq("OrgAdminId", olduser.OrgAdminId) & Builders<User>.Filter.Eq("OrganizationId", user.OrganizationId), Builders<User>.Update.Set("OrgAdminId", user.Id));
         return new ServiceResult { Success = true, Message = "User Updated", Data = user };
        }

        //private method to check if user is already registered or not
        private bool IsUserAlreadyRegistered(string phoneNumber, string nationalId)
        {
            var FilterPhoneNumber = Builders<User>.Filter.Eq("userMobileNumber", phoneNumber) | Builders<User>.Filter.Eq("userNationalId", nationalId);
            var listuser = _users.Find(FilterPhoneNumber).ToList();
            return listuser.Count > 0;
        }
        private bool IsOrganizationAlreadyRegistered(string id)
        {
            var filter = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = _organizations.Find(filter).ToList();
            return list.Count > 0;
        }

        public ServiceResult UpdateUser(string id, User newuser)
        {
            User user = GetByUserId(id).Data as User;

            if (user is null)
                return new ServiceResult { Success = false, Message = "User Not Found" };

            var res = _users.FindOneAndReplace(Builders<User>.Filter.Eq("_id", ObjectId.Parse(user.Id)), newuser);
            return new ServiceResult { Success = true, Message = "User Updated", Data = res };
        }

        public ServiceResult DeleteUser(string id, string organizationId)
        {
            if (IsOrganizationAlreadyRegistered(organizationId))
            {
                var user = GetByUserId(id);
                if (user is null)
                    return new ServiceResult { Success = false, Message = "User Not Found" };
                var res = _users.DeleteOne(Builders<User>.Filter.Eq("_id", ObjectId.Parse(id)));
                return new ServiceResult { Success = true, Message = "User Deleted", Data = res };
            }
            return new ServiceResult { Success = false, Message = "Organization Not Found" };
        }

        public ServiceResult GetUserByOrganization(string id, string organizationId)
        {
            if (IsOrganizationAlreadyRegistered(organizationId))
            {
                var user = GetByUserId(id);
                if (user is null)
                    return new ServiceResult { Success = false, Message = "User Not Found" };

                return new ServiceResult { Success = true, Message = "User Found", Data = user };
            }
            return new ServiceResult { Success = false, Message = "Organization Not Found" };

        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool RegisterOrganization(Organization organization)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOrganization(Organization organization)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrganization(string id)
        {
            throw new NotImplementedException();
        }

        public Organization GetOrganization<Organization>(string id)
        {
            throw new NotImplementedException();
        }

        public List<Organization> GetOrganizations()
        {
            throw new NotImplementedException();
        }

        public List<Permission> GetPermissions()
        {
            var list = _permissions.Find(FilterDefinition<Permission>.Empty).ToList();
            return list;
        }

      
    }

}


