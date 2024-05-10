// i wante to create Permission 
// add using statments 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using AgentCheckListApi.Enums;
namespace AgentCheckListApi.Model
{
    public class Permission
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRequired]        
        public UserRole Role { get; set; }
        [BsonRequired]
        [BsonElement("IsActive")]
        public bool IsActive { get; set; }
        public string? UserMobileNumber { get; set; }
    }
}