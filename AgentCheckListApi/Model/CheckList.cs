// Create Check List 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace AgentCheckListApi.Model
{
    [BsonIgnoreExtraElements]
    public class CheckList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]

        public string OrganizationId { get; set; }
        public string Organization_name { get; set; }
        public List<Question> Questions { get; set; }


    }
}