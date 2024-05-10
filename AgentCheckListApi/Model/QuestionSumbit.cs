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

    public class QuestionSumbit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InspectionId { get; set; } = ObjectId.GenerateNewId().ToString();
        [BsonRequired]
        public required string question { get; set; }
        [BsonRequired]
        public required Answer answer { get; set; } = Answer.NA;
        [BsonRequired]
        public required string note { get; set; }
        public bool? HasImage { get; set; }
        public bool? HasVoice { get; set; }

    }
}