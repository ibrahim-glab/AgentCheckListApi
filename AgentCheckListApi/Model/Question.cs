// add Question Entity 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using AgentCheckListApi.Enums;
namespace AgentCheckListApi.Model
{
    [BsonIgnoreExtraElements]

    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? InspectionId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRequired]
        public required string question { get; set; }
        public string? note { get; set; }
        
        public readonly List<Answer> answers   = new List<Answer>(){Answer.Complied, Answer.NonComplied, Answer.NA}; 
        public bool? avail_Camera { get; set; }
        public bool? avail_Voice{ get; set; }

    }
}