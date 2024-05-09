// Implement IRegisteratoinService
// c:/Users/Bebo/source/repos/AgentCheckListApi/AgentCheckListApi/Services/RegisteratoinService.cs
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgentCheckListApi.Services
{
    public class RegisteratoinService : IRegisterationService
    {
        //replace With MongoDb Service
        private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Organization> _organizations;
        public RegisteratoinService(MongoDbService<Organization> mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _users = _mongoDbService.GetCollection<User>("users");
            _organizations = _mongoDbService.GetCollection<Organization>("organizations");
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
        public User GetByUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
            var user = _users.Find(filter).FirstOrDefault();
            return user;
        }
        public Organization GetOrganizationById(string id)
        {
            var filter = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var organization = _organizations.Find(filter).FirstOrDefault();
            return organization;
        }

        public ServiceResult RegisterUser(string id,User user)
        {
            if (!IsUserAlreadyRegistered(user.UserMobileNumber , user.UserNationalId))
            {
                if (IsOrganizationAlreadyRegistered(id)){
                user.OrganizationId = id;
                 _users.InsertOne(user);
                return new ServiceResult{Success = true, Message = "User Registered" , Data = user};
                }
                return new ServiceResult{Success = false, Message = "Organization Not Found"};
            }
            return new ServiceResult{Success = false, Message = "User Already Registered with this Phone Number or National Id"};
           
        }

        //private method to check if user is already registered or not
        private bool IsUserAlreadyRegistered(string phoneNumber , string nationalId)
        {
            var FilterPhoneNumber = Builders<User>.Filter.Eq("userMobileNumber", phoneNumber) | Builders<User>.Filter.Eq("userNationalId", nationalId);
            var listuser = _users.Find(FilterPhoneNumber).ToList();
           return listuser.Count > 0;  
        }
        private bool IsOrganizationAlreadyRegistered(string id){
            var filter = Builders<Organization>.Filter.Eq("_id", ObjectId.Parse(id));
            var list = _organizations.Find(filter).ToList();
            return list.Count > 0;
        }

        public ServiceResult UpdateUser(string id,User newuser)
        {
            var user = GetByUserById(id);
            if(user is null)
                return new ServiceResult{Success = false, Message = "User Not Found"};
            
            var res=  _users.FindOneAndReplace(Builders<User>.Filter.Eq("_id", ObjectId.Parse(user.Id)) , newuser );
            return new ServiceResult{Success = true, Message = "User Updated" , Data = res};
        }

        public ServiceResult DeleteUser(string id , string organizationId)
        {
           if (IsOrganizationAlreadyRegistered(organizationId))
           {
            var user = GetByUserById(id);
            if(user is null)
                return new ServiceResult{Success = false, Message = "User Not Found"};
              var res=  _users.DeleteOne(Builders<User>.Filter.Eq("_id", ObjectId.Parse(id)));
              return new ServiceResult{Success = true, Message = "User Deleted" , Data = res};
           }
           return new ServiceResult{Success = false, Message = "Organization Not Found"};
        }

        public ServiceResult GetUserByOrganization(string id , string organizationId)
        {
         if (IsOrganizationAlreadyRegistered(organizationId))
            {
                var user = GetByUserById(id);
                if(user is null)
                    return new ServiceResult{Success = false, Message = "User Not Found"};

                return new ServiceResult{Success = true, Message = "User Found" , Data = user};
            }
            return new ServiceResult{Success = false, Message = "Organization Not Found"};

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
    }
    
    }


       