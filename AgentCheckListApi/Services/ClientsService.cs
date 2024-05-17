using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AgentCheckListApi.Services
{
    public class ClientsService : IClientsService

    {
        private readonly IMongoCollection<AgentCheckListApi.Model.Directory> _Directory;
        private readonly IMongoQueryable<AgentCheckListApi.Model.Directory> _DirectoryQueryable;
        private readonly IMongoQueryable<Organization> _organizatoinQueryable;
        private readonly IRegisterationService registerationService;

        public ClientsService(IOptions<MongoDb> configuration, IRegisterationService registerationService)
        {
            var client = new MongoClient(configuration.Value.ConnectionString);
            var database = client.GetDatabase(configuration.Value.Database);
            _Directory = database.GetCollection<AgentCheckListApi.Model.Directory>("Directory");
            _DirectoryQueryable = _Directory.AsQueryable();
            _organizatoinQueryable = database.GetCollection<Organization>("organizations").AsQueryable();
            this.registerationService = registerationService;
        }

        public ServiceResult Create(Model.Directory directory)
        {
            try
            {
                if (!_organizatoinQueryable.Any(o => o.Id == directory.OrganizationId))
                    return new ServiceResult { Success = false, Message = "The Organization Not Found" };

                _Directory.InsertOne(directory);
                return new ServiceResult { Success = true, Data = directory, Message = "The Client is created" };
            }
            catch (System.Exception e)
            {
                return new ServiceResult { Success = false, Message = e.Message };

            }
        }

        public ServiceResult Delete(string id)
        {
            var ServiceResult = _Directory.DeleteOne(x => x.id == id);
            if (!(ServiceResult.DeletedCount == 1))
                return new ServiceResult { Success = false, Message = "Directory Not Found" };
            return new ServiceResult { Success = true, Message = "Directory Deleted" };
        }

        public ServiceResult Get(string id)
        {
            var client = _DirectoryQueryable.FirstOrDefault(d => d.id == id);
            if (client is null)
                return new ServiceResult { Success = false, Message = "Clinet Not Found" };
            return new ServiceResult { Success = true, Data = client };
        }

        public ServiceResult GetDirectories()
        {
            try
            {

                var directories = _Directory.Find(x => true).ToList();
                return new ServiceResult { Success = true, Data = directories };
            }
            catch (System.Exception e)
            {

                return new ServiceResult { Success = false, Message = e.Message };
            }
        }

        public ServiceResult Update(Model.Directory directory)
        {

            try
            {
                var directoryFind = _Directory.Find(x => x.id == directory.id).FirstOrDefault();

                if (directoryFind == null)
                {
                    return new ServiceResult { Success = false, Message = "Directory not found" };
                }
                var result = _Directory.ReplaceOne(d => d.id == directory.id, directory);
                return new ServiceResult { Success = true, Data = directory };
            }
            catch (System.Exception e)
            {
                return new ServiceResult { Success = false, Message = e.Message };
            }
        }
        public ServiceResult GetDirectoriesForUser(string id)
        {
            var serviceResult = registerationService.GetByUserId(id);
            if (!serviceResult.Success)
                return new ServiceResult { Success = false, Message = serviceResult.Message };
            var user = serviceResult.Data as User;
            var doir = _DirectoryQueryable.All(x => x.OrganizationId == user.OrganizationId);
            var Directories = _DirectoryQueryable.Where(x => x.OrganizationId == user.OrganizationId).ToList();
            return new ServiceResult { Success = true, Data = Directories };
        }

    }
}
