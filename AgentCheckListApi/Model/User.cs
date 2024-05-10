/*Organization  ID
Org Admin ID
User Status
Business User ID
Username  
User Password
User Mobile Number
User National ID
User Email
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace AgentCheckListApi.Model{
[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonElement("organizationId")]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? OrganizationId { get; set; }
    [BsonElement("orgAdminId")]
    [BsonRepresentation(BsonType.ObjectId)]

    public string? OrgAdminId { get; set; }
    [Required]
    [BsonElement("userStatus")]
    public bool IsActive { get; set; }
    [Required]
    [BsonElement("businessUserId")]
    public int BusinessUserId { get; set; }
    [Required]
    [BsonElement("username")]
    [BsonRepresentation(BsonType.String)]
    public required string Username { get; set; }
    [Required]
    [BsonElement("userPassword")]
    [BsonRepresentation(BsonType.String)]
    public required string UserPassword { get; set; }
    [Required]
    [BsonElement("userMobileNumber")]
    public required string UserMobileNumber { get; set; }
    [Required]
    [BsonElement("userNationalId")]
    public required string UserNationalId { get; set; }
    [Required]
    public required string UserEmail { get; set; }
}
}