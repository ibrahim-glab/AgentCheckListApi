// create Form Model 

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
    public class Form
    {
    public DateTime SubmissionDate { get; set; } = DateTime.Now;
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? SubmissionID { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrganizationID { get; set; }
    public string OrganizationName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]

    public string UserID { get; set; }
    public string Username { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]

    public string ClientID { get; set; } = "000000000000000000000000";
    public string ClientName { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]

    public string ChecklistID { get; set; }
    public string ChecklistName { get; set; }
    [Range(1, 10)]
    public int ChecklistProgress { get; set; }
    public bool ChecklistStatus { get; set; }
    public List<QuestionSumbit> Questions { get; set; } 
   
    }
}