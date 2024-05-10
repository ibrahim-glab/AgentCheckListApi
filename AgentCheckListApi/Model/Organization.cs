using System;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
namespace AgentCheckListApi.Model
{
//this is Mongo Class  
[BsonIgnoreExtraElements]
public class Organization
{
  
// add id 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null!;
    

    [Required]
    public string Organization_name { get; set; } = null!;
    //active status
    [Required]
    public bool Organization_status { get; set; } 
    [Required]
    // License Id and Unique
    public required string LicenseId { get; set; }
    [Required]

    public string OrganizationType { get; set; }
    [Required]


    public string? OrganizationFinancialId { get; set; } =null!;
    public int? FinancialLimitFrom { get; set; } =null!;
    public int? FinancialLimitTo { get; set; } =null!;
    public int? BankAccount { get; set; } = null!;
}
}