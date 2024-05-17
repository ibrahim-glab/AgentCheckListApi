using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AgentCheckListApi.Model
{
    public class Directory
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public required string OrganizationId { get; set; }
        [BsonRequired]
        public required string OrganizationName { get; set; }
        [BsonRequired]
        public required string CustomerType { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? AccountId { get; set; }
        public string? AccountName { get; set; }
        [BsonRequired]
        public required string AccountAddress { get; set; }
        [BsonRequired]
        public required string ContactMobileNumber { get; set; }

        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public required string ContactId { get; set; }

    }
}